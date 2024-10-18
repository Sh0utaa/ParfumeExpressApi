$(document).ready(function () {
    $('#registerForm').on('submit', function (e) {
        e.preventDefault();
        const registerData = {
            email: $('input[name="email"]').val(),
            password: $('input[name="password"]').val(),
        };
        $.ajax({
            url: 'https://localhost:7046/register',
            type: 'POST',
            contentType: 'application/json',

            data: JSON.stringify(registerData),
            success: function (response) {
                // Handle success (e.g., store token, redirect)
                console.log(response);
                alert('Registration successful');
            },
            error: function () {
                alert('Invalid Registration');
                console.log(response);
            }
        });
    });
});