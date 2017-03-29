angular.module('gallery', ['ngRoute', 'ngCookies'])
    .config([
        '$locationProvider', '$routeProvider', "$httpProvider",
        function ($locationProvider, $routeProvider, $httpProvider) {
            $routeProvider
                /* admin */
                .when('/Angular/home', {
                    templateUrl: '/views/Angular/home.html',
                    controller: 'TextController'
                })
                .when('/Angular/gallery', {
                    templateUrl: '/views/Angular/gallery.html',
                    controller: 'GalleryController'
                })
                .when('/Angular/addImg', {
                    templateUrl: '/views/Angular/addImg.html',
                    controller: 'AddImageController',
                    resolve: {
                        "check": function ($location, $rootScope) {
                            if (!$rootScope.loggedIn) {
                                $location.path('/');
                            }
                        }
                    }
                })
                .when('/Angular/register', {
                    templateUrl: '/views/Angular/registerForm.html',
                    controller: 'RegisterController'
                })
                .when('/Angular/createAlbum',
                {
                    templateUrl: '/views/Angular/createAlbum.html',
                    controller: 'AlbumController',
                    resolve: {
                        "check": function ($location, $rootScope) {
                            if (!$rootScope.loggedIn) {
                                $location.path('/');
                            }
                        }
                    }
                })
                .when('/Angular/myAlbums',
                {
                    templateUrl: '/views/Angular/myAlbums.html',
                    controller: 'MyAlbumsController',
                    resolve: {
                        "check": function ($location, $rootScope) {
                            if (!$rootScope.loggedIn) {
                                $location.path('/');
                            }
                        }
                    }
                })
                .when('/Angular/cart',
                {
                    templateUrl: '/views/Angular/cart.html',
                    controller: 'CartController',
                    resolve: {
                        "check": function ($location, $rootScope) {
                            if (!$rootScope.loggedIn) {
                                $location.path('/');
                            }
                        }
                    }
                })
                .otherwise({
                    redirectTo: '/Angular/gallery'
                });

            $httpProvider.interceptors.push("AuthHttpResponseInterceptor");
            $locationProvider.html5Mode(true);
        }
    ])
.factory('AuthHttpResponseInterceptor', ["$q", "$location",
    function ($q, $location) {
        return {
            request: function (request) {
                $(".spinner").show();
                return request;
            },
            response: function (response) {
                $(".spinner").hide();
                if (response.status === 401) {
                    console.log("Response 401");
                };
                return response || $q.when(response);
            },
            responseError: function (rejection) {
                if (rejection.status === 401) {
                    console.log("Response Error 401", rejection);
                    $location.path('/login').search('returnUrl', $location.path());
                }
                return $q.reject(rejection);
            }
        }
    }])
.controller('MyAlbumsController', ["$scope", "dataCenter",
    function ($scope, dataCenter) {
        $scope.albums = {
            available: [],
            selected: {}
        };

        $scope.getImages = function () {
            dataCenter.getImagesForAlbum($scope.albums.selected.id)
                    .then(function (respons) {
                        $scope.data = respons.data;
                    });
        };

        $scope.remove = function (id) {
            dataCenter.remove(id).then(function () {
                $scope.getImages();
            });
        };

        function getAlbums() {
            dataCenter.getAlbumsForCurrentUser().then(function (respons) {
                $scope.albums.available = respons.data;
                $scope.albums.selected = $scope.albums.available[0];
                $scope.getImages();
            });
        };

        getAlbums();
    }])
.controller('AlbumController', ["$scope", "dataCenter",
    function ($scope, dataCenter) {
        $scope.create = function () {
            dataCenter.createAlbum($scope.albumName)
            .then(function () {
                $scope.albumName = '';
                alert("The album has been created!");
            }, function () {
                $scope.albumName = '';
                alert("There was an error during creating the album");
            });
        };
    }])
.controller('LoginController', ["$scope", "$routeParams", "$location", "loginService", "authService", "rootService",
    function ($scope, $routeParams, $location, loginService, authService, rootService) {
        $scope.loginForm = {
            emailAddress: '',
            password: '',
            returnUrl: $routeParams.returnUrl,
            loggedIn: true,
            role: ''
        };

        function greeting() {
            var data = authService.getCredentials();
            if (!!data.userEmail) {
                rootService.setRoots(data.userEmail, data.role, data.userId);
            };
            $scope.loginForm.emailAddress = rootService.getRoots().email;
            $scope.loginForm.role = rootService.getRoots().role;
            $scope.loginForm.loggedIn = rootService.getRoots().loggedIn;
        }

        greeting();

        $scope.login = function () {
            loginService.login($scope.loginForm.emailAddress, $scope.loginForm.password)
                .then(function (result) {
                    $scope.loginForm.loggedIn = true;
                    rootService.setRoots(result.data.Email, result.data.Role, result.data.Id);
                    authService.setCredentials(
                        result.data.Id,
                        result.data.Email,
                        result.data.Password,
                        result.data.RoleId,
                        result.data.Role);
                    if ($scope.loginForm.returnUrl === undefined) {
                        $location.path('/Angular/gallery');
                    } else {
                        $location.path($scope.loginForm.returnUrl);
                    }
                },
                function () {
                    $scope.loginForm.loggedIn = false;
                    $scope.loginForm.emailAddress = '';
                    rootService.removeRoots();
                });
        };

        $scope.logout = function () {
            authService.clearCredentials();
            $scope.loginForm.loggedIn = false;
            rootService.removeRoots();
            $location.path('/');
        }
    }])
.service("rootService", ["$rootScope", function($rootScope) {
        function setRoots(email, role, id) {
            $rootScope.loggedIn = true;
            $rootScope.email = email;
            $rootScope.role = role;
            $rootScope.id = id;
        };

        function getRoots() {
            return {
                email: $rootScope.email,
                role: $rootScope.role,
                loggedIn: $rootScope.loggedIn
            }
        }

        function removeRoots() {
            $rootScope.loggedIn = false;
            $rootScope.email = '';
            $rootScope.role = '';
            $rootScope.id = '';
        };

        return {
            setRoots: setRoots,
            removeRoots: removeRoots,
            getRoots: getRoots
        }
    }])
.service("authService", ["$cookies", function ($cookies) {
    function setCredentials(id, email, password, roleId, role) {
        $cookies.put('userId', id);
        $cookies.put('userEmail', email);
        $cookies.put('userpPassword', password);
        $cookies.put('roleId', roleId);
        $cookies.put('role', role);
    };

    function getCredentials() {
        return {
            userId: $cookies.get('userId'),
            userEmail: $cookies.get('userEmail'),
            userpPassword: $cookies.get('userpPassword'),
            roleId: $cookies.get('roleId'),
            role: $cookies.get('role')
        }
    };

    function clearCreadentials() {
        $cookies.remove('userId');
        $cookies.remove('userEmail');
        $cookies.remove('userpPassword');
        $cookies.remove('roleId');
        $cookies.remove('role');
    };

    return {
        setCredentials: setCredentials,
        getCredentials: getCredentials,
        clearCredentials: clearCreadentials
    }
}])
.controller('RegisterController', ["$scope", "$location", "RegistrationFactory",
    function ($scope, $location, RegistrationFactory) {
        $scope.registerForm = {
            emailAddress: '',
            password: '',
            confirmPassword: '',
            registrationFailure: false
        };

        $scope.register = function () {
            var result = RegistrationFactory($scope.registerForm.emailAddress,
                $scope.registerForm.password,
                $scope.registerForm.confirmPassword);
            result.then(function (result) {
                if (result.data) {
                    $location.path('/Angular/gallery');
                } else {
                    $scope.loginForm.registrationFailure = true;
                }
            }, function () {
                $scope.loginForm.registrationFailure = true;
            });
        }
    }])
.service('loginService', ["$http",
    function ($http) {
        function login(emailAddress, password) {
            var respons = $http({
                method: "POST",
                url: 'http://localhost:54287/Account/Login',
                data: {
                    email: emailAddress,
                    password: password
                },
                headers: { 'Accept': 'application/json' }
            });
            return respons;
        }

        return {
            login: login
        }
    }])
.factory('RegistrationFactory', ["$http",
    function ($http) {
        return function (emailAddress, password, confirmPassword) {
            var respons = $http({
                method: "POST",
                url: 'http://localhost:54287/Account/Register',
                data: {
                    email: emailAddress,
                    password: password,
                    confirmPassword: confirmPassword
                },
                headers: { 'Accept': 'application/json' }
            });
            return respons;
        }
    }])
.controller("GalleryController", ["$scope", "dataCenter", "$http",
    function ($scope, dataCenter) {
        $scope.albums = {
            available: [],
            selected: {}
        };

        $scope.clearClassFull = function($index) {
            var images = $scope.data;
            for (var i = 0; i < images.length; i++) {
                if (i !== $index) {
                    images[i].isPrew = false;
                }
            }
        };

        $scope.addToCart = function (img) {
            dataCenter.addToCart(img).then(function() {
                getAll();
            });
        }

        function getAll() {
            dataCenter.getAll().then(function (response) {
                $scope.data = response.data;
            });
        };

        getAll();
    }])
.controller("AddImageController", ["$scope", "dataCenter", function ($scope, dataCenter) {
    $scope.albums = {
        available: [],
        selected: {}
    };

    dataCenter.getAlbumsForCurrentUser().then(function (respons) {
        $scope.albums.available = respons.data;
        $scope.albums.available.splice(0, 1);
        $scope.albums.selected = $scope.albums.available[0];
    });

    $scope.clearClassFull = function ($index) {
        var images = $scope.data;
        for (var i = 0; i < images.length; i++) {
            if (i !== $index) {
                images[i].isPrew = false;
            }
        }
    }

    $scope.add = function () {
        dataCenter.add($scope.img.name, $scope.img.data, $scope.img.description, $scope.albums.selected.id, $scope.img.isTradable, $scope.img.price)
            .then(function () {
                $scope.img.name = "";
                $scope.img.data = "";
                $scope.img.description = "";
                $scope.img.price = "";
            });
    };
}])
.controller("TextController", ["$scope", "$rootScope", "dataCenter",
    function ($scope, $rootScope, dataCenter) {
    $scope.isEdit = false;
    $scope.text = "The site presents a gallery on the theme of interior design. Interior Design is the definitive resource for interior designers, architects and other design pros, featuring groundbreaking projects, innovative new products, ...";

    $scope.goEdit = function () {
        if ($rootScope.role === 'Admin') {
            $scope.isEdit = true;
        }
    }

    $scope.applyEdit = function () {
        $scope.isEdit = false;
    }
}])
.controller("CartController", ["$scope", "dataCenter",
    function ($scope, dataCenter) {
        function getCart() {
            dataCenter.getCart().then(function(respons) {
                $scope.data = respons.data;
                $scope.sum = getSum();
            });
        };

        getCart();

        function getSum() {
            var sum = 0;
            for (var i = 0; i < $scope.data.length; i++) {
                sum += $scope.data[i].Price;
            }
            return sum;
        };

        $scope.clearClassFull = function ($index) {
            var images = $scope.data;
            for (var i = 0; i < images.length; i++) {
                if (i !== $index) {
                    images[i].isPrew = false;
                }
            }
        };
    }])
.service("dataCenter", ["$http", "$rootScope", function ($http, $rootScope) {
    function getAll() {
        var respons = $http({
            url: 'http://localhost:54287/Image/GetImages'
        });
        return respons;
    };

    function add(fileName, data, description, albumId, isTradable, price) {
        var respons = $http({
            method: "POST",
            url: 'http://localhost:54287/Image/AddImageAjax',
            data: {
                fileName: fileName,
                data: data,
                description: description,
                albumId: albumId,
                isTradable: isTradable === undefined ? false : true,
                price: price
            },
            headers: { 'Accept': 'application/json' }
        });
        return respons;
    };

    function remove(id) {
        return $http({
            method: "POST",
            url: 'http://localhost:54287/Image/DeleteFileAjax',
            data: { id: id },
            headers: { 'Accept': 'application/json' }
        });
    };

    function getAlbumsForCurrentUser() {
        var respons = $http({
            method: "POST",
            url: 'http://localhost:54287/Album/GetAlbums',
            data: {
                id: $rootScope.id
            },
            headers: { 'Accept': 'application/json' }
        });
        return respons;
    };

    function createAlbum(albumName) {
        var respons = $http({
            method: "POST",
            url: 'http://localhost:54287/Album/CreateAlbum',
            data: {
                albumName: albumName,
                userEmail: $rootScope.email
            },
            headers: { 'Accept': 'application/json' }
        });
        return respons;
    };

    function getImagesForAlbum(albumId) {
        var respons = $http({
            method: "POST",
            url: 'http://localhost:54287/Image/GetImagesForAlbum',
            data: {
                albumId: albumId,
                userId: $rootScope.id
            },
            headers: { 'Accept': 'application/json' }
        });
        return respons;
    };

    function getDescription() {
        var respons = $http({
            url: 'http://localhost:54287/Image/GetDescription'
        });
        return respons;
    };

    //var cart = [];

    //function addToCart(img) {
    //    cart.push(img);
    //}
    //function removeFormCart($index) {
    //    cart.slice($index, 1);
    //}
    //function getCart() {
    //    return cart;
    //}

    function addToCart(img) {
        var respons = $http({
            method: "POST",
            url: 'http://localhost:54287/Image/AddToCart',
            data: {
                imageId: img.Id,
                userId: $rootScope.id
            },
            headers: { 'Accept': 'application/json' }
        });
        return respons;
    }

    function getCart() {
        var respons = $http({
            method: "POST",
            url: 'http://localhost:54287/Image/GetCart',
            data: {
                userId: $rootScope.id
            },
            headers: { 'Accept': 'application/json' }
        });
        return respons;
    }

    return {
        getAll: getAll,
        add: add,
        remove: remove,
        getAlbumsForCurrentUser: getAlbumsForCurrentUser,
        createAlbum: createAlbum,
        getImagesForAlbum: getImagesForAlbum,
        getDescription: getDescription,
        addToCart: addToCart,
        //removeFormCart: removeFormCart,
        getCart: getCart
    }
}])
.directive("fileread", [function () {
    return {
        scope: {
            fileread: "="
        },
        link: function (scope, element, attributes) {
            element.bind("change", function (changeEvent) {
                var reader = new FileReader();
                reader.onload = function (loadEvent) {
                    scope.$apply(function () {
                        scope.fileread = loadEvent.target.result;
                    });
                }
                reader.readAsDataURL(changeEvent.target.files[0]);
            });
        }
    }
}])
