import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import "./EditProfile.css";
import {
  IoArrowBackCircleOutline,
  IoArrowBackCircle,
  IoAdd,
  IoAddOutline,
} from "react-icons/io5";
import authLogin from "../../utils/consts/AuthUserLogin";
import userData from "../../data/userData.json";

function EditProfile() {
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [bio, setBio] = useState("");
  const [profileImage, setProfileImage] = useState(null);
  const [login, setLogin] = useState("");
  const [email, setEmail] = useState("");
  const [isExitHovered, setIsExitHovered] = useState(false);
  const [isHovered, setIsHovered] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchUserData = () => {
      const foundUser = userData.find((user) => user.login === authLogin);
      if (foundUser) {
        setLogin(foundUser.login);

        const [firstName, lastName] = foundUser.username.split(" ");

        setFirstName(firstName);
        setLastName(lastName || "");

        setEmail(foundUser.email);
        setBio(foundUser.bio);
        setProfileImage(foundUser.profileImage);
      }
    };

    fetchUserData();
  }, []);

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onloadend = () => {
        setProfileImage(reader.result);
      };
      reader.readAsDataURL(file);
    }
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    navigate(-1);
  };

  return (
    <div className="edit-profile">
      <form onSubmit={handleSubmit} className="edit-profile-form">
        <div className="navigate-container">
          <div
            className="close-creation-icon general-text"
            onMouseEnter={() => setIsExitHovered(true)}
            onMouseLeave={() => setIsExitHovered(false)}
            onClick={() => navigate(-1)}
          >
            {isExitHovered ? (
              <IoArrowBackCircle />
            ) : (
              <IoArrowBackCircleOutline />
            )}
            <span className="general-text back-text">Back</span>
          </div>
          <h2 className="edit-profile-text general-text">Edit Profile</h2>
          <button type="submit" className="save-button">
            Save
          </button>
        </div>
        <div className="wrapper">
          <div className="profile-picture-container">
            <img
              src={profileImage || "/default-profile.png"}
              alt="Profile"
              className="profile-picture"
            />
            <div className="upload-overlay">
              <label
                htmlFor="image-upload"
                onMouseEnter={() => setIsHovered(true)}
                onMouseLeave={() => setIsHovered(false)}
              >
                {isHovered ? <IoAdd /> : <IoAddOutline />}
              </label>
              <input
                type="file"
                id="image-upload"
                className="file-input"
                accept="image/*"
                onChange={handleImageChange}
                style={{ display: "none" }}
              />
            </div>
          </div>
        </div>
        <input
          type="text"
          name="login"
          placeholder="Login"
          value={login}
          onChange={(e) => setLogin(e.target.value)}
          required
        />
        <input
          type="text"
          name="first-name"
          placeholder="First Name"
          value={firstName}
          onChange={(e) => setFirstName(e.target.value)}
          required
        />
        <input
          type="text"
          name="last-name"
          placeholder="Last Name"
          value={lastName}
          onChange={(e) => setLastName(e.target.value)}
          required
        />
        <input
          type="email"
          name="email"
          placeholder="Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
        <textarea
          className="bio-text"
          type="text"
          name="bio"
          placeholder="Bio"
          value={bio}
          onChange={(e) => setBio(e.target.value)}
          onInput={(e) => {
            if (e.target.scrollHeight > e.target.clientHeight) {
              e.target.style.height = "auto";
              e.target.style.height = `${e.target.scrollHeight}px`;
            }
          }}
          required
        />
      </form>
    </div>
  );
}

export default EditProfile;
