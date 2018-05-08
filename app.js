'use strict';

// Use of AngularJS as a controller

let app = angular.module('recommenderApp', ['ngMaterial', 'ngMessages']) // create the "app" and inyect dependencies
.config(function($mdThemingProvider) { // configure color theme
	$mdThemingProvider.theme('default').primaryPalette('teal').accentPalette('blue');
	$mdThemingProvider.setDefaultTheme('default');
});

// Actual controller
app.controller('recommenderController', function($scope, $http, $mdToast){
	$scope.creatingDatabase = false; // if the database is beign created
	$scope.recommendingGames = false; // if the process of recommendation is beign executed
	$scope.userGames = { // lists of user games by input
		liked: [],
		disliked: []
	};
	$scope.listOfGames = []; // list of recommended games
	$scope.recommendationsQuantity = 20; // number of recommended games to display

	// Call the API to create and populate the database with the dataset of Steamspy (check api/movielens.js for details)
	$scope.createDatabase = function(){
		$scope.creatingDatabase = true;
		$http.get('api/create')
		.then(function(res){
			$scope.creatingDatabase = false;
			$mdToast.show(
				$mdToast.simple()
					.textContent('Database created succesfully')
					.hideDelay(4000)
			);
		}, function(res){
			console.error('Database connection error', res);
		});
	};
	
	// Obtain a list of games with title similar to query
	// The parameter is a query passed by an autocomplete input
	// Returns a promise which, eventually, returns a list of games
	$scope.query = function(query){
		return $http.get(`/api/games?q=${query}`).
    	then(function(res){
    		return res.data;
    	}, function(res){
    		console.error('Database connection error', res);
    	});
	};

	// Add the movie object in the autocomplete input to the list of user liked/disliked games
	$scope.addLikedGame = function(){
		$scope.userGames.liked.push($scope.selectGame.liked);
		$scope.selectGame.liked = undefined;
	};
	$scope.addDislikedGame = function(){
		$scope.userGames.disliked.push($scope.selectGame.disliked);
		$scope.selectGame.disliked = undefined;
	};

	// Delete the object that represent a movie in the list of user liked/disliked games
	// The parameter is the index to delete
	$scope.deleteLikedGame = function(index){
		$scope.userGames.liked.splice(index, 1);
	};
	$scope.deleteDislikedGame = function(index){
		$scope.userGames.disliked.splice(index, 1);
	};

	// Score all the games by the algorithm of Content Based Recommender
	// For details check: https://www.analyticsvidhya.com/blog/2015/08/beginners-guide-learn-content-based-recommender-systems/
	$scope.recommend = function(){
		$scope.recommendingGames = true;
		let gameMatrix = [],
			totalAtributes = [],
			normalizedMatrix = [],
			userProfile = [],
			userProfileNormalized = [],
			DF = [],
			IDF = [],
			userPrediction = [];
		console.log("Recommending...");
		$http.get('/api/gamesGenre').
		then(function(res){
			console.log('Total results: ', res.data.length);
			// Game matrix (0's & 1's)
			res.data.forEach(game => {
				if(gameMatrix[game.GameID] === undefined)
					gameMatrix[game.GameID] = [];
				gameMatrix[game.GameID][game.genreId] = 1;
			});
			gameMatrix.forEach(game => {
				for(let i = 0; i<=20; i++) // We know that exists 20 genres
					if(!game[i])
						game[i] = 0;
			});
			// Total atributes
			for(let i in gameMatrix){
				totalAtributes[i] = gameMatrix[i].reduce((a, b) => a + b, 0);
			}
			console.log('Matrix created!');
			console.log('Showing the info for gameId 1:', gameMatrix["1"]);
			console.log('totalAtributes for gameId 1:', totalAtributes["1"]);
			// Normalized game matrix
			gameMatrix.forEach((game, i) => {
				normalizedMatrix[i] = [];
				game.forEach((value, j) => {
					normalizedMatrix[i][j] = value / Math.sqrt(totalAtributes[i]);
				});
			});
			console.log('Normalized matrix created!');
			console.log('Showing the normalization for gameId 1:', normalizedMatrix["1"]);
			// User vector
			gameMatrix.forEach((game, i) => {
				userProfile[i] = 0;
			});
			$scope.userGames.liked.forEach(obj => {
				userProfile[obj.GameID] = 1;
			});
			$scope.userGames.disliked.forEach(obj => {
				userProfile[obj.GameID] = -1;
			});
			console.log('User profile vector created: ', userProfile);
			// User profile ("normalized") vector
			for(let i = 0; i <= 20; i++){
				let sum = 0;
				normalizedMatrix.forEach((game, j) => {
					sum += game[i] * userProfile[j];
				});
				userProfileNormalized[i] = sum;
			}
			console.log('User profile normalized created: ', userProfileNormalized);
			// DF vector
			for(let i = 0; i <= 20; i++){
				let sum = 0;
				gameMatrix.forEach(game => {
					sum += game[i];
				});
				DF[i] = sum;
			}
			console.log('DF vector created: ', DF);
			// IDF vector
			DF.forEach((value, i) => {
				IDF[i] = Math.log(gameMatrix.length / value);
			});
			console.log('IDF vector created: ', IDF);
			// User prediction vector
			normalizedMatrix.forEach((game, i) => {
				let sum = 0;
				for(let j = 1; j <= 20; j++)
					sum += game[j] * userProfileNormalized[j] * IDF[j];
				userPrediction[i] = sum;
			});
			console.log('User prediction vector created: ', userPrediction);
			// Obtain the complete list of games
			$http.get('/api/games').
			then(function(res){
				$scope.listOfGames = res.data;
				// Add the obtained score (predicted value)
				$scope.listOfGames.forEach(objGame => {
					objGame.score = userPrediction[objGame.GameID];
				});
				// Sort by best score
				$scope.listOfGames.sort((a,b) => b.score - a.score);
				console.log('List of games with predicted scores created: ', $scope.listOfGames);
				$scope.recommendingGames = false;
			}, function(res){
				console.error('Database connection error', res);
			});
		}, function(res){
			console.error('Database connection error', res);
		});
	};
});
