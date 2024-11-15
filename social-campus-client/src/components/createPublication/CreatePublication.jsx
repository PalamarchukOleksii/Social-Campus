import React, { useState } from "react";
import PropTypes from "prop-types";
import { toast } from "react-toastify";
import ShortProfile from "../shortProfile/ShortProfile";
import "./CreatePublication.css";

function CreatePublication(props) {
  const [publicationText, setPublicationText] = useState("");
  const [image, setImage] = useState(null);
  const [imagePreview, setImagePreview] = useState(null);

  const handleInputChange = (e) => {
    setPublicationText(e.target.value);
  };

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setImage(file);
      const reader = new FileReader();
      reader.onloadend = () => {
        setImagePreview(reader.result);
      };
      reader.readAsDataURL(file);
    }
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    if (publicationText.trim()) {
      const newPublication = {
        id: props.getMaxPublicationId() + 1,
        description: publicationText,
        imageUrl: image ? URL.createObjectURL(image) : null,
        creationTime: new Date().toISOString(),
        likesCount: 0,
        comments: [],
        username: props.user.username,
        login: props.user.login,
        profileImage: props.user.profileImage,
      };

      props.setPublications([newPublication, ...props.publications]);

      setPublicationText("");
      setImage(null);
      setImagePreview(null);
    } else {
      toast("Publication text cannot be empty.");
    }
  };

  return (
    <div className="create-publication">
      <ShortProfile
        username={props.user.username}
        login={props.user.login}
        profileImage={props.user.profileImage}
        redirectOnClick={false}
      />
      <form className="create-form" onSubmit={handleSubmit}>
        <textarea
          className="publication-text"
          type="text"
          placeholder="Publication text"
          value={publicationText}
          onChange={handleInputChange}
          required
        />
        <div className="image-upload">
          <input
            type="file"
            accept="image/*"
            onChange={handleImageChange}
            id="file-input"
          />
          {imagePreview && (
            <div className="image-preview-container">
              <img src={imagePreview} alt="Preview" className="image-preview" />
            </div>
          )}
        </div>
        <div className="controles">
          <button className="publish-button" type="submit">
            Publish
          </button>
        </div>
      </form>
    </div>
  );
}

CreatePublication.propTypes = {
  user: PropTypes.shape({
    username: PropTypes.string.isRequired,
    login: PropTypes.string.isRequired,
    profileImage: PropTypes.string,
  }).isRequired,
  publications: PropTypes.arrayOf(
    PropTypes.shape({
      id: PropTypes.oneOfType([PropTypes.number, PropTypes.string]).isRequired,
      description: PropTypes.string,
      imageUrl: PropTypes.string,
      creationTime: PropTypes.string,
      likesCount: PropTypes.number,
      comments: PropTypes.arrayOf(PropTypes.object).isRequired,
      username: PropTypes.string,
      login: PropTypes.string,
      profileImage: PropTypes.string,
    })
  ).isRequired,
  setPublications: PropTypes.func.isRequired,
  getMaxPublicationId: PropTypes.func.isRequired,
};

export default CreatePublication;
