'use strict';

/**
 * @ngdoc function
 * @name goinstantApp.controller:MainCtrl
 * @description
 * # MainCtrl
 * Controller of the goinstantApp
 */
angular.module('goinstantApp')
  .controller('MainCtrl', function ($scope) {
    $scope.awesomeThings = [
      'HTML5 Boilerplate',
      'AngularJS',
      'Karma',
      'Haskell'
    ];
  });
