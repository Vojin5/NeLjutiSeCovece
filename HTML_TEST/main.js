let img = null;

const btn = document.body.querySelector(".btn");
btn.addEventListener("click", async () => {
    const obj = {
        username: "makishaokkk",
        password: "sifra1921u2",
        email: "saisjaij@gmail.com",
        imageBase64Encoded: img
    };
    await fetch("http://localhost:5295/User/register", {
        method: "POST",
        body: JSON.stringify(obj),
        headers: {
            "Content-Type":"application/json"
        }
    })
});

const getbtn = document.body.querySelector('.get-btn');
getbtn.addEventListener("click", async () => {
    const obj = {
        username: "makishaokkk",
        password: "sifra1921u2"
    }
    const user = await (await fetch("http://localhost:5295/User/login", {
        method: "POST",
        body: JSON.stringify(obj),
        headers: {
            "Content-Type":"application/json"
        }
    })).json();
    const imagePreview = document.body.querySelector(".image-preview");
    console.log(user);
    console.log(imagePreview);
    imagePreview.src = "data:image/png;base64," + user.image;
    imagePreview.style = "block";
});

const imageInput = document.body.querySelector(".file-input");
imageInput.addEventListener("change", () => {
    const imagePreview = document.body.querySelector(".image-preview");
    const file = imageInput.files[0];
    const reader = new FileReader();

    reader.onloadend = () => {
        imagePreview.src = reader.result;
        imagePreview.style = "block";
        img = reader.result.toString().replace(/^data:(.*,)?/, '');
        console.log(img);
    }

    if (file) {
        reader.readAsDataURL(file);
    }
    else {
        imagePreview.src = "";
    }
});