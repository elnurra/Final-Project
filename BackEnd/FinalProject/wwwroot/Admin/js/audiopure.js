const player = document.getElementById("player");
const audio = document.getElementById("audio");
const playPause = document.getElementById("play-pause");
const prev = document.getElementById("prev");
const next = document.getElementById("next");
const playlist = document.getElementById("songs");
const progressBar = document.getElementById("progress-bar");
const progress = document.querySelector(".progress");
const volume = document.getElementById("volume");
const currentTime = document.querySelector("#current-time");
const duration = document.querySelector("#duration");
const heartIcon = document.querySelector("#salam");
const chooseRandom = document.querySelector(".random-take");
const maxValue = playlist.children.length;

let currentSong = 0;



playlist.addEventListener("click", (e) => {
    if (e.target.tagName === "LI") {
        currentSong = Array.from(playlist.children).indexOf(e.target);
        audio.src = e.target.getAttribute("data-src");
        playPause.innerHTML = '<i class="fa-solid fa-pause"></i>';
        audio.play();
        setActiveSong(currentSong);
    } else {
        audio.pause();
        playPause.innerHTML = '<i class="fa-solid fa-play"></i>';
    }
});

playPause.addEventListener("click", () => {
    if (audio.paused) {
        audio.src = playlist.children[currentSong].getAttribute("data-src");
        audio.play();
        setActiveSong(currentSong);
        playPause.innerHTML = '<i class="fa-solid fa-pause"></i>';
    } else {
        audio.pause();
        playPause.innerHTML = '<i class="fa-solid fa-play"></i>';
    }
});



volume.addEventListener("input", (e) => {
    audio.volume = e.target.value;
});

audio.addEventListener("timeupdate", (e) => {
    const percent = (audio.currentTime / audio.duration) * 100;
    progress.style.width = `${percent}%`;
    currentTime.textContent = formatTime(audio.currentTime);
    duration.textContent = formatTime(audio.duration);
});

function formatTime(time) {
    const minutes = Math.floor(time / 60);
    const seconds = Math.floor(time % 60);
    return `${minutes}:${seconds.toString().padStart(2, "0")}`;
}

progressBar.addEventListener("click", (e) => {
    const percent =
        (e.clientX - progressBar.offsetLeft) / progressBar.offsetWidth;
    audio.currentTime = percent * audio.duration;
    progress.style.width = `${percent}%`;
});

audio.addEventListener("ended", (e) => {
    if (currentSong === playlist.children.length - 1) {
        currentSong = 0;
    } else {
        currentSong++;
    }
    audio.src = playlist.children[currentSong].getAttribute("data-src");
    audio.play();
    setActiveSong(currentSong);
});

const setActiveSong = (index) => {
    // remove "active" class from previously active song
    playlist.querySelector(".active-song").classList.remove("active-song");

    // add "active" class to currently playing song
    const activeSong = playlist.querySelectorAll("li")[index];
    activeSong.classList.add("active-song");
};