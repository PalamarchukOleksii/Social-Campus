import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import "./EditProfile.css";

// Подключим FontAwesome для иконки стрелочки
import { FaArrowLeft } from "react-icons/fa";

function EditProfile() {
  const [username, setUsername] = useState("");
  const [name, setName] = useState("");
  const [bio, setBio] = useState("");
  const [profileImage, setProfileImage] = useState(null);

  const navigate = useNavigate();

  // Функция для обработки изменения изображения
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

  // Функция для обработки отправки формы
  const handleSubmit = (e) => {
    e.preventDefault();
    // Здесь можно добавить логику для сохранения данных (например, API запрос)
    console.log("Profile updated:", { username, name, bio, profileImage });
  };

  return (
    <div className="edit-profile">
      <h2>Edit Profile</h2>
      <form onSubmit={handleSubmit}>
        <label>
          Name:
          <input
            type="text"
            name="name"
            value={name}
            onChange={(e) => setName(e.target.value)}
          />
        </label>

        <label>
          Username:
          <input
            type="text"
            name="username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
          />
        </label>

        <label>
          Bio:
          <textarea
            name="bio"
            value={bio}
            onChange={(e) => setBio(e.target.value)}
          />
        </label>

        <label htmlFor="file-input">
          Profile Image:
        </label>
        <input
          type="file"
          id="file-input"
          accept="image/*"
          onChange={handleImageChange}
        />

        {profileImage && (
          <div className="image-preview">
            <img src={profileImage} alt="Profile Preview" />
          </div>
        )}

        <button type="submit">Save</button>
      </form>

      <button
        className="back-button"
        onClick={() => navigate(-1)} // Возвращает на предыдущую страницу
      >
        <FaArrowLeft /> Back
      </button>
    </div>
  );
}

export default EditProfile;
