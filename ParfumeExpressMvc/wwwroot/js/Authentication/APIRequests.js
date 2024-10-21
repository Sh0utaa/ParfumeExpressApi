document.addEventListener('DOMContentLoaded', function () {
    const loginForm = document.getElementById('loginForm');
    const registerForm = document.getElementById('registerForm');
    const loginBtn = document.querySelector("label.login");

    // Handle login form submission
    loginForm.addEventListener('submit', function (e) {
        e.preventDefault();

        const email = loginForm.querySelector('input[name="email"]').value;
        const password = loginForm.querySelector('input[name="password"]').value;

        const loginData = {
            email: email,
            password: password
        };

        fetch('https://localhost:7046/api/auth/login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(loginData)
        })
            .then(response => response.json())
            .then(data => {
                // Store the token in sessionStorage
                sessionStorage.setItem('jwtToken', data.token.result);
                console.log('login successfull');
                window.location.href = '/'; // Redirects to the homepage
            })
            .catch(error => {
                console.error('Error:', error);
                alert('Login failed: ' + error);
            });
    });

    // Handle registration form submission
    // Assuming you have the registerForm element already selected
    registerForm.addEventListener('submit', function (e) {
        e.preventDefault();

        const username = registerForm.querySelector('input[name="username"]').value.trim();
        const email = registerForm.querySelector('input[name="email"]').value.trim();
        const password = registerForm.querySelector('input[name="password"]').value.trim();

        console.log('Captured Values:', { username, email, password }); // Debug log

        // Validate that all fields are filled
        if (!username || !email || !password) {
            alert('All fields are required!');
            return;
        }

        const registerData = { username, email, password };

        fetch('https://localhost:7046/api/auth/register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(registerData)
        })
            .then(response => {
                if (!response.ok) {
                    return response.json().then(errResponse => {
                        // Throwing an error with the server response
                        throw new Error(JSON.stringify(errResponse));
                    });
                }
                return response.json();
            })
            .then(data => {
                loginBtn.click();
                console.log('Success Response:', data);
                alert('Registration successful');
                // Optionally, redirect or perform another action here
                // window.location.href = '/'; // Redirect to homepage, for example
            })
            .catch(error => {
                console.error('Error Response:', error);
                // Parse the error response if it's in JSON format
                try {
                    const errorDetails = JSON.parse(error.message);
                    if (errorDetails.errors && errorDetails.errors.length > 0) {
                        const errorMessages = errorDetails.errors.map(err => `${err.description}`).join('\n');
                        alert('Registration failed:\n' + errorMessages);
                    } else {
                        alert('Registration failed: ' + errorDetails.error);
                    }
                } catch (e) {
                    alert('Registration failed: ' + error.message);
                }
            });
    });
});
