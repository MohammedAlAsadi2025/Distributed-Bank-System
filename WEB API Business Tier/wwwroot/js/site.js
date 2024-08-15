// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function loadView(status) {
    var apiUrl;

    switch (status) {
        case "authview":
            apiUrl = '/api/login/authview';
            break;
        case "error":
            apiUrl = '/api/login/error';
            break;
        case "profile":
            apiUrl = '/api/profile/view';
            break;
        case "account":
            apiUrl = '/api/account/view';
            break;
        case "transfer":
            apiUrl = '/api/transfer/view';
            break;
        case "history":
            apiUrl = '/api/history/view';
            break;
        case "manage":
            apiUrl = '/api/manage/view';
            break;
        case "usertransactions":
            apiUrl = '/api/usertransactions/view';
            break;       
        case "logs":
            apiUrl = '/api/logs/view';
            break;
        case "logout":
            apiUrl = '/api/logout';
            break;
        default:
            apiUrl = '/api/login/defaultview';
            break;
    }

    console.log("Hello " + apiUrl);

    fetch(apiUrl)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(data => {
            // Handle the data from the API
            document.getElementById('main').innerHTML = data;
            if (status === "logout") {
                document.getElementById('LogoutButton').style.display = "none";
            }
        })
        .catch(error => {
            // Handle any errors that occurred during the fetch
            console.error('Fetch error:', error);
        });

}

/*
document.addEventListener('DOMContentLoaded', function () {
    console.log("DOMContentLoaded triggered");
    if (getCookie("IsAdmin") === "true") {
        document.getElementById("ManageButton").style.display = "block";
        document.getElementById("UserTransactionsButton").style.display = "block";
        document.getElementById("LogsButton").style.display = "block";
        console.log("IsAdmin cookie value:", getCookie("IsAdmin"));
    }
});*/

/*
function getCookie(name) {
    var value = "; " + document.cookie;
    var parts = value.split("; " + name + "=");
    if (parts.length == 2) return parts.pop().split(";").shift();
}*/





function performAuth() {

    var name = document.getElementById('SName').value;
    var password = document.getElementById('SPass').value;
    var data = {
        UserName: name,
        PassWord: password
    };
    console.log(data);  // changed from error to log for clarity
    const apiUrl = '/api/login/auth';

    const headers = {
        'Content-Type': 'application/json', // Specify the content type as JSON if you're sending JSON data
    };

    const requestOptions = {
        method: 'Post',
        headers: headers,
        body: JSON.stringify(data) // Convert the data object to a JSON string
    };

    fetch(apiUrl, requestOptions)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            const jsonObject = data;
            if (jsonObject.login) {
                loadView("authview");
                document.getElementById('LogoutButton').style.display = "block";

                // Check if it's the admin that logged in
                if (name === "admin" && password === "admin111") {
                    // Display the admin buttons
                    document.getElementById("ManageButton").style.display = "block";
                    document.getElementById("UserTransactionsButton").style.display = "block";
                    document.getElementById("LogsButton").style.display = "block";
                }
            } else {
                loadView("error");
            }
        })
        .catch(error => {
            // Handle any errors that occurred during the fetch
            console.error('Fetch error:', error);
        });

}

function searchByUsername() {
    const name = document.getElementById('usernameSearch').value;
    fetch(`/api/manage/searchByName?name=${name}`)
        .then(response => response.json())
        .then(data => {
            console.log("Parsed data:", data);

            // Access the properties using the correct casing
            document.getElementById('userNameDisplay').textContent = data.userName;
            document.getElementById('emailDisplay').textContent = data.email;
            document.getElementById('firstNameDisplay').textContent = data.firstName;
            document.getElementById('lastNameDisplay').textContent = data.lastName;
            document.getElementById('phoneDisplay').textContent = data.phone;

        })
        .catch(error => {
            console.error('Error:', error);
            alert('Failed to fetch user details. Please try again.');
        });
}



function searchByAccountNo() {
    const accountNo = document.getElementById('accountNoSearch').value;
    fetch(`/api/manage/searchByAccountNo?accountNo=${accountNo}`)
        .then(response => response.json())
        .then(data => {
            // Again, assuming the data returned has fields matching the UserProfile structure
            //document.getElementById('userNameDisplay').textContent = data.UserName;
            //document.getElementById('emailDisplay').textContent = data.Email;
            document.getElementById('firstNameDisplay').textContent = data.FirstName;
            document.getElementById('lastNameDisplay').textContent = data.LastName;
            //document.getElementById('phoneDisplay').textContent = data.Phone;

        })
        .catch(error => {
            console.error('Error:', error);
            alert('Failed to fetch account details. Please try again.');
        });
}




document.addEventListener("DOMContentLoaded", loadView);
/*
const loginButton = document.getElementById('LoginButton');
loginButton.addEventListener('click', loadView);

const aboutButton = document.getElementById('AboutButton');
aboutButton.addEventListener('click', loadView("about"));

const logoutButton = document.getElementById('LogoutButton');
logoutButton.addEventListener('click', loadView("logout"));*/



