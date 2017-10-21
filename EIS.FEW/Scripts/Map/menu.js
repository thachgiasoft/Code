$(document).ready(function () {
     var url = window.location;  
      $('.navbar-nav li a[href="' + url + '"]').parent().addClass('active');
       $('.navbar-nav li a').filter(function () {
          return this.href == url;		
     }).addClass('active').parent().addClass('active').parent().parent().addClass('active');
  });