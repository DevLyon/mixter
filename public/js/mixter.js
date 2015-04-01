document.addEventListener("DOMContentLoaded", function(event) { 
	var MainViewModel = function(){
		var self = this;
		
		self.email = ko.observable();
		
		self.connection = function(){
		alert("Hello " + self.email());
		};
	};
	
	var mainViewModel = new MainViewModel();
	ko.applyBindings(mainViewModel);
});