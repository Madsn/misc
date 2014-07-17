'use strict';

/**
 * @ngdoc overview
 * @name goinstantApp
 * @description
 * # goinstantApp
 *
 * Main module of the application.
 */
angular
  .module('goinstantApp', [
    'ngAnimate',
    'ngCookies',
    'ngResource',
    'ngRoute',
    'ngSanitize',
    'ngTouch',
    'ui.bootstrap',
    'goangular'
  ]).config(function ($routeProvider, $goConnectionProvider) {
    $routeProvider
      .when('/', {
        templateUrl: 'views/main.html',
        controller: 'MainCtrl'
      })
      .when('/about', {
        templateUrl: 'views/about.html',
        controller: 'AboutCtrl'
      })
      .otherwise({
        redirectTo: '/'
      });
    $goConnectionProvider.$set('https://goinstant.net/a6f678bb341d/my-application');
  });
