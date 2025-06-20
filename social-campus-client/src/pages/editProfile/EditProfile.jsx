import React, { useState, useEffect, useRef } from "react";
import { useNavigate, useParams } from "react-router-dom";
import "./EditProfile.css";
import {
  IoArrowBackCircleOutline,
  IoArrowBackCircle,
  IoAddCircle,
  IoAddCircleOutline,
  IoCloseCircle,
  IoCloseCircleOutline,
} from "react-icons/io5";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import useAuth from "../../hooks/useAuth";
import { toast } from "react-toastify";

const EDIT_PROFILE_URL = "/api/users/update";
const GET_USER_BY_LOGIN_URL = "/api/users/by-login/";

function EditProfile() {
  const { login } = useParams();
  const { auth } = useAuth();
  const axios = useAxiosPrivate();

  const prevLoginRef = useRef(login);

  const [userId, setUserId] = useState();
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [bio, setBio] = useState("");
  const [profileImageFile, setProfileImageFile] = useState(null);
  const [profileImagePreview, setProfileImagePreview] = useState(null);
  const [userLogin, setUserLogin] = useState("");
  const [email, setEmail] = useState("");
  const [isExitHovered, setIsExitHovered] = useState(false);
  const [isHovered, setIsHovered] = useState(false);
  const [removeImgHovered, setRemoveImgHovered] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const fetchUserData = async () => {
      try {
        const { data } = await axios.get(`${GET_USER_BY_LOGIN_URL}${login}`);

        setUserId(data.id?.value || "");
        setFirstName(data.firstName || "");
        setLastName(data.lastName || "");
        setBio(data.bio || "");
        setEmail(data.email || "");
        setUserLogin(data.login || "");

        if (data.profileImageUrl) {
          setProfileImagePreview(data.profileImageUrl);

          const imageResponse = await fetch(data.profileImageUrl);
          const blob = await imageResponse.blob();
          const filename =
            data.profileImageUrl.split("/").pop() || "profile.jpg";
          const file = new File([blob], filename, { type: blob.type });
          setProfileImageFile(file);
        } else {
          setProfileImagePreview(null);
          setProfileImageFile(null);
        }
      } catch (error) {
        console.error("Error fetching user data:", error);
      }
    };

    fetchUserData();
  }, [login]);

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      if (!["image/jpeg", "image/png"].includes(file.type)) {
        toast.error("Only JPG and PNG files are allowed.");
        return;
      }

      setProfileImageFile(file);
      setProfileImagePreview(URL.createObjectURL(file));
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    try {
      const formData = new FormData();

      formData.append("callerId.value", auth.shortUser.id.value);
      formData.append("userId.value", userId);
      formData.append("login", userLogin);
      formData.append("email", email);
      formData.append("firstName", firstName);
      formData.append("lastName", lastName);
      formData.append("bio", bio);

      if (profileImageFile) {
        formData.append("profileImage", profileImageFile);
      } else {
        formData.append("profileImage", "");
      }

      const response = await axios.patch(EDIT_PROFILE_URL, formData, {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      });

      if (response.status === 200) {
        toast.success("Profile updated successfully!");
        navigate(`/profile/${userLogin}`);

        if (prevLoginRef.current !== userLogin) {
          window.location.reload();
        }
      }
    } catch (error) {
      console.error("Error updating profile:", error);
      toast.error("Failed to update profile. Please try again.");
    }
  };

  const removeImage = () => {
    setProfileImageFile(null);
    setProfileImagePreview(null);
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
              src={profileImagePreview || "/default-profile.png"}
              alt="Profile"
              className="profile-picture"
            />
            <div className="control-img">
              <div className="upload-overlay">
                <label
                  htmlFor="image-upload"
                  onMouseEnter={() => setIsHovered(true)}
                  onMouseLeave={() => setIsHovered(false)}
                >
                  {isHovered ? <IoAddCircle /> : <IoAddCircleOutline />}
                </label>
                <input
                  type="file"
                  id="image-upload"
                  className="file-input"
                  accept=".jpg,.jpeg,.png"
                  onChange={handleImageChange}
                  style={{ display: "none" }}
                />
              </div>
              <div className="remove-overlay">
                <label
                  onMouseEnter={() => setRemoveImgHovered(true)}
                  onMouseLeave={() => setRemoveImgHovered(false)}
                  onClick={removeImage}
                >
                  {removeImgHovered ? (
                    <IoCloseCircle />
                  ) : (
                    <IoCloseCircleOutline />
                  )}
                </label>
              </div>
            </div>
          </div>
        </div>
        <input
          className="login-input"
          type="text"
          name="login"
          placeholder="Login"
          value={userLogin}
          onChange={(e) => setUserLogin(e.target.value)}
          required
        />
        <input
          className="first-name-input"
          type="text"
          name="first-name"
          placeholder="First Name"
          value={firstName}
          onChange={(e) => setFirstName(e.target.value)}
          required
        />
        <input
          className="last-name-input"
          type="text"
          name="last-name"
          placeholder="Last Name"
          value={lastName}
          onChange={(e) => setLastName(e.target.value)}
          required
        />
        <input
          className="email-input"
          type="email"
          name="email"
          placeholder="Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
        />
        <textarea
          className="bio-text"
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
        />
      </form>
    </div>
  );
}

export default EditProfile;
