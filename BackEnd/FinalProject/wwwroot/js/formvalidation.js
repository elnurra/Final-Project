let input = document.getElementById('newLetter');
let button = document.getElementById('submitLetter');

button.disabled = true;   // Make button disabled initially

input.addEventListener('keyup', function (event) {

    let val = event.target.value;  // input's current value

    if (val === '') {
        button.disabled = true;  // Make button disabled
    }
    else {
        button.disabled = false;  // Make button enabled 
    }

});

