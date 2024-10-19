//$(document).ready(function () {
//    $('#loginForm').on('submit', function (e) {
//        e.preventDefault();

//        const loginData = {
//            email: $('input[name="email"]').val(),
//            password: $('input[name="password"]').val(),
//        };

//        $.ajax({
//            url: 'https://localhost:7046/api/auth/login',
//            type: 'POST',
//            contentType: 'application/json',
//            data: JSON.stringify(loginData),
//            success: function (response) {
//                // Store the token in localStorage
//                sessionStorage.setItem('jwtToken', response.token.result);
//                alert('Login successful!');
//            },
//            error: function (xhr) {
//                console.error('Error:', xhr.responseText);
//                alert('Login failed: ' + xhr.responseText);
//            }
//        });
//    });
//});


//$(document).ready(function () {
//    $('#registerForm').on('submit', function (e) {
//        e.preventDefault();
//        const registerData = {
//            email: $('input[name="email"]').val(),
//            password: $('input[name="password"]').val(),
//        };
//        $.ajax({
//            url: 'https://localhost:7046/register',
//            type: 'POST',
//            contentType: 'application/json',

//            data: JSON.stringify(registerData),
//            success: function (response) {
//                // Handle success (e.g., store token, redirect)
//                console.log(response);
//                alert('Registration successful');
//            },
//            error: function () {
//                alert('Invalid Registration');
//                console.log(response);
//            }
//        });
//    });
//});