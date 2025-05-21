const body = document.querySelector("body");
const darkLight = document.querySelector("#darkLight");
const sidebar = document.querySelector(".sidebar");
const mainContent = document.querySelector("main");
const asideContent = document.querySelector("aside");
const sidebarOpen = document.querySelector("#sidebarOpen");
const sidebarClose = document.querySelector(".collapse_sidebar");
const sidebarExpand = document.querySelector(".expand_sidebar");

const toggleSidebar = (collapse) => {
    if (collapse) {
        sidebar.classList.add("close", "hoverable");
        mainContent.classList.add("expanded");
        asideContent.classList.add("expanded");
    } else {
        sidebar.classList.remove("close", "hoverable");
        mainContent.classList.remove("expanded");
        asideContent.classList.remove("expanded");
    }
};

sidebarOpen.addEventListener("click", () => {
    sidebar.classList.toggle("close");
    mainContent.classList.toggle("expanded");
    asideContent.classList.toggle("expanded");
});

sidebarClose.addEventListener("click", () => toggleSidebar(true));
sidebarExpand.addEventListener("click", () => toggleSidebar(false));

sidebar.addEventListener("mouseenter", () => {
    if (sidebar.classList.contains("hoverable")) {
        sidebar.classList.remove("close");
        mainContent.classList.remove("expanded");
        asideContent.classList.remove("expanded");
    }
});

sidebar.addEventListener("mouseleave", () => {
    if (sidebar.classList.contains("hoverable")) {
        sidebar.classList.add("close");
        mainContent.classList.add("expanded");
        asideContent.classList.add("expanded");
    }
});

//darkLight.addEventListener("click", () => {
//    body.classList.toggle("dark");
//    darkLight.classList.toggle("bx-sun");
//    darkLight.classList.toggle("bx-moon");
//});

darkLight.addEventListener("click", () => {
    body.classList.toggle("dark");
    const isDark = body.classList.contains("dark");
    localStorage.setItem("darkMode", isDark ? "enabled" : "disabled");

    darkLight.classList.toggle("bx-sun", !isDark);
    darkLight.classList.toggle("bx-moon", isDark);
});


window.addEventListener("resize", () => {
    if (window.innerWidth < 768) {
        toggleSidebar(true);
    } else {
        toggleSidebar(false);
    }
});

const profile = document.querySelector('.navbar_content');
const dropdown = document.querySelector('.dropdown_wrapper');
profile.addEventListener('click', () => {
    dropdown.classList.remove('none');
    dropdown.classList.toggle('hide');
})

document.addEventListener("click", (event) => {
    const isClickInsideDropdown = dropdown.contains(event.target);
    const isProfileClicked = profile.contains(event.target);
    if (!isClickInsideDropdown && !isProfileClicked) {
        dropdown.classList.add('hide');
        dropdown.classList.add('dropdown_wrapper--fade-in');
    }
})

document.addEventListener("DOMContentLoaded", function () {
    const darkMode = localStorage.getItem("darkMode");
    if (darkMode === "enabled") {
        body.classList.add("dark");
        darkLight.classList.add("bx-moon");
        darkLight.classList.remove("bx-sun");
    } else {
        darkLight.classList.add("bx-sun");
        darkLight.classList.remove("bx-moon");
    }

    const submenuItems = document.querySelectorAll(".submenu_item");
    submenuItems.forEach(item => {
        item.addEventListener("click", function (event) {
            event.preventDefault(); // Prevent default anchor behavior
            const submenu = this.nextElementSibling; // Get the submenu
            if (submenu && submenu.classList.contains("submenu")) {
                submenu.classList.toggle("active"); // Toggle submenu visibility
            }
        });
    });
});


// File Upload Script

document.addEventListener("DOMContentLoaded", () => {
    const dropZone = document.querySelector(".dropzone-area");
    const fileInput = document.querySelector("input[name='file']");
    const fileInfoContainer = document.querySelector(".file-info");
    const fileLimit = 25 * 1024 * 1024;

    const allowedExtensions = ["xls", "xlsx"];
    ["dragenter", "dragover", "dragleave", "drop"].forEach(event => {
        dropZone.addEventListener(event, e => e.preventDefault());
    });

    //fileInput.addEventListener("change", (e) => {
    //    handleFileSelection(e.target.files);
    //    if (e.target.files.length > 0) {
    //        e.target.form.submit();
    //    }
    //});


    dropZone.addEventListener("dragover", () => dropZone.classList.add("drag-over"));
    dropZone.addEventListener("dragleave", () => dropZone.classList.remove("drag-over"));

    dropZone.addEventListener("drop", (e) => {
        dropZone.classList.remove("drag-over");
        const files = e.dataTransfer.files;
        handleFileSelection(files);
    });

    fileInput.addEventListener("change", (e) => {
        handleFileSelection(e.target.files);
    });
    function handleFileSelection(files) {
        if (files.length > 1) {
            alert("Only one file can be uploaded at a time!");
            fileInput.value = "";
            return;
        }

        const file = files[0];
        const fileExtension = file.name.split('.').pop().toLowerCase();

        //console.log("Selected file: ", file);

        if (!allowedExtensions.includes(fileExtension)) {
            alert("Only Excel files (.xls, .xlsx) are allowed!");
            fileInput.value = "";
            return;
        }

        if (file.size > fileLimit) {
            alert("File size exceeds 25MB limit!");
            fileInput.value = "";
            return;
        }

        updateFileInfo(file);
    }
    function updateFileInfo(file) {
        if (fileInfoContainer) {
            fileInfoContainer.innerHTML = `
                <p>File Selected: <strong>${file.name}</strong></p>
                <p>Size: ${(file.size / 1024 / 1024).toFixed(2)} MB</p>`;
        }
    }
});
