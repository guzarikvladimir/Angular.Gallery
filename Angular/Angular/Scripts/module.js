﻿angular.module('gallery', ['ngRoute', 'ngCookies'])
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
                .otherwise({
                    redirectTo: '/Angular/gallery'
                });

            $httpProvider.interceptors.push("AuthHttpResponseInterceptor");
            $locationProvider.html5Mode(true);
        }
    ])
.factory('AuthHttpResponseInterceptor', ["$q", "$location", "$rootScope", "authService",
    function ($q, $location, $rootScope, authService) {
        return {
            response: function (response) {

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

        function getAlbums() {
            dataCenter.getAlbumsForCurrentUser().then(function (respons) {
                $scope.albums.available = respons.data;
                $scope.albums.selected = $scope.albums.available[0];
                getImages();
            });
        };

        getAlbums();
    }])
.controller('AlbumController', ["$scope", "dataCenter",
    function ($scope, dataCenter) {
        $scope.create = function () {
            dataCenter.createAlbum($scope.albumName)
            .then(function () {
                alert("The album has been created!");
            }, function () {
                alert("There was an error during creating the album");
            });
        };
    }])
.controller('LoginController', ["$scope", "$routeParams", "$location", "$rootScope", "loginService", "authService",
    function ($scope, $routeParams, $location, $rootScope, loginService, authService) {
        $scope.loginForm = {
            emailAddress: '',
            password: '',
            returnUrl: $routeParams.returnUrl,
            loggedIn: true,
            role: ''
        };

        var data = authService.getCredentials();
        if (!!data.userEmail) {
            $rootScope.loggedIn = true;
            $rootScope.email = data.userEmail;
            $rootScope.role = data.role;
            $rootScope.id = data.userId;
        };
        $scope.loginForm.emailAddress = $rootScope.email;
        $scope.loginForm.role = $rootScope.role;
        $scope.loginForm.loggedIn = $rootScope.loggedIn;

        $scope.login = function () {
            loginService.login($scope.loginForm.emailAddress, $scope.loginForm.password)
                .then(function (result) {
                    $scope.loginForm.loggedIn = true;
                    $rootScope.loggedIn = $scope.loginForm.loggedIn;
                    if (result.data.Role === 'Admin') {
                        $rootScope.role = 'Admin';
                    } else {
                        $rootScope.role = 'User';
                    }
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
                        $scope.loginForm.loggedIn = true;
                        $rootScope.loggedIn = $scope.loginForm.loggedIn;
                    });
        };

        $scope.logout = function () {
            authService.clearCredentials();
            $scope.loginForm.loggedIn = false;
            $rootScope.loggedIn = false;
            $rootScope.email = '';
            $rootScope.role = '';
            $rootScope.id = '';
            $location.path('/');
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
        $scope.filter = false;

        $scope.albums = {
            available: [],
            selected: {}
        };

        $scope.toggleFilter = function () {
            dataCenter.getAlbumsForCurrentUser().then(function (respons) {
                $scope.albums.available = respons.data;
                $scope.albums.selected = $scope.albums.available[0];
            });
            $scope.filter = !$scope.filter;
        }

        $scope.remove = function (url) {
            dataCenter.remove(url).then(function () {
                getAll();
            });
        };

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
        $scope.albums.selected = $scope.albums.available[0];
    });

    $scope.add = function () {
        dataCenter.add($scope.img.name, $scope.img.data, $scope.img.description, $scope.albums.selected.id, $scope.img.isTradable)
            .then(function (response) {
                $scope.img.name = "";
                $scope.img.data = "";
                $scope.img.description = "";
            });
    };
}])
.controller("TextController", ["$scope", "$rootScope", function ($scope, $rootScope) {
    $scope.text = "The site presents a gallery on the theme of interior design. " +
        "Interior Design is the definitive resource for interior designers, architects and other design pros, featuring groundbreaking projects, innovative new products, ...";
    $scope.isEdit = false;

    $scope.goEdit = function () {
        if ($rootScope.role === 'Admin') {
            $scope.isEdit = true;
        }
    }

    $scope.applyEdit = function () {
        $scope.isEdit = false;
    }
}])
.service("dataCenter", ["$http", "$rootScope", function ($http, $rootScope) {
    function getAll() {
        var respons = $http({
            url: 'http://localhost:54287/Image/GetImages'
        });
        return respons;
    };

    function add(fileName, data, description, albumId, isTradable) {
        var respons = $http({
            method: "POST",
            url: 'http://localhost:54287/Image/AddImageAjax',
            data: {
                fileName: fileName,
                data: data,
                description: description,
                albumId: albumId,
                isTradable: isTradable === undefined ? false : true
            },
            headers: { 'Accept': 'application/json' }
        });
        return respons;
    };

    function remove(url) {
        return $http({
            method: "POST",
            url: 'http://localhost:54287/Image/DeleteFileAjax',
            data: { url: url },
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
                albumId: albumId
            },
            headers: { 'Accept': 'application/json' }
        });
        return respons;
    };

    return {
        getAll: getAll,
        add: add,
        remove: remove,
        getAlbumsForCurrentUser: getAlbumsForCurrentUser,
        createAlbum: createAlbum,
        getImagesForAlbum: getImagesForAlbum
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
