import React, { useEffect, useState } from "react";
import PropTypes from "prop-types";
import { toast } from "react-toastify";
import ShortProfile from "../shortProfile/ShortProfile";
import {
  IoImageOutline,
  IoImage,
  IoCloseOutline,
  IoClose,
  IoArrowBackCircleOutline,
  IoArrowBackCircle,
} from "react-icons/io5";
import "./CreatePublication.css";
import axios from "../../utils/api/AxiosBase";
import useAuth from "../../hooks/useAuth";

const CREATE_PUBLICATION_URL = "/api/publications/create";

function CreatePublication(props) {
  const [publicationText, setPublicationText] = useState("");
  const [image, setImage] = useState(null);
  const [imagePreview, setImagePreview] = useState(null);
  const [isHovered, setIsHovered] = useState(false);
  const [isCloseHovered, setIsCloseHovered] = useState(false);
  const [isExitHovered, setIsExitHovered] = useState(false);
  const [loading, setLoading] = useState(true);
  const { auth } = useAuth();
  const [user, setUser] = useState({});

  useEffect(() => {
    const currentUser = auth?.shortUser || {};
    setUser(currentUser);
    setLoading(false);
  }, [auth]);

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

  const handleSubmit = async (e) => {
    e.preventDefault();

    let imageData = "";
    if (image) {
      const reader = new FileReader();
      reader.readAsDataURL(image);
      reader.onloadend = async () => {
        imageData = reader.result;
        try {
          await axios.post(CREATE_PUBLICATION_URL, {
            description: publicationText,
            creatorId: {
              value: user.id.value,
            },
            imageData: imageData,
          });
        } catch (error) {
          const { response } = error;

          if (response?.data?.error) {
            response.data.error.forEach((err) => toast.error(err.message));
          } else if (response?.data?.detail) {
            toast.error(response.data.detail);
          } else {
            console.error(error);
            toast.error("An unexpected error occurred.");
          }
        } finally {
          setPublicationText("");
          setImage(null);
          setImagePreview(null);
          props.onCloseClick();
        }
      };
    } else {
      try {
        await axios.post(CREATE_PUBLICATION_URL, {
          description: publicationText,
          creatorId: {
            value: user.id.value,
          },
          imageData: "",
        });
      } catch (error) {
        const { response } = error;

        if (response?.data?.error) {
          response.data.error.forEach((err) => toast.error(err.message));
        } else if (response?.data?.detail) {
          toast.error(response.data.detail);
        } else {
          console.error(error);
          toast.error("An unexpected error occurred.");
        }
      } finally {
        setPublicationText("");
        setImage(null);
        setImagePreview(null);
        props.onCloseClick();
      }
    }
  };

  const removeImage = () => {
    setImage(null);
    setImagePreview(null);
  };

  const closeCreatePublication = () => {
    setPublicationText("");
    setImage(null);
    setImagePreview(null);
    props.onCloseClick();
  };

  if (loading) {
    return <></>;
  }

  return (
    <div className="create-publication">
      <div
        className="close-creation-icon general-text"
        onMouseEnter={() => setIsExitHovered(true)}
        onMouseLeave={() => setIsExitHovered(false)}
        onClick={closeCreatePublication}
      >
        {isExitHovered ? <IoArrowBackCircle /> : <IoArrowBackCircleOutline />}
        <span className="general-text back-text">Back</span>
      </div>
      <ShortProfile
        username={user.firstName + " " + user.lastName}
        login={user.login}
        profileImage={user.profileImage}
        redirectOnClick={false}
      />
      <form className="create-form" onSubmit={handleSubmit}>
        <textarea
          className="publication-text"
          type="text"
          placeholder="Your publication text..."
          value={publicationText}
          onChange={handleInputChange}
          onInput={(e) => {
            if (e.target.scrollHeight > e.target.clientHeight) {
              e.target.style.height = "auto";
              e.target.style.height = `${e.target.scrollHeight}px`;
            }
          }}
          required
        />
        {imagePreview && (
          <div className="image-preview-container">
            <img src={imagePreview} alt="Preview" className="image-preview" />
            <div
              className="remove-image-icon general-text"
              onMouseEnter={() => setIsCloseHovered(true)}
              onMouseLeave={() => setIsCloseHovered(false)}
              onClick={removeImage}
            >
              {isCloseHovered ? <IoClose /> : <IoCloseOutline />}
            </div>
          </div>
        )}
        <div className="controles">
          <label
            className="image-upload-icon general-text"
            htmlFor="image-upload"
            onMouseEnter={() => setIsHovered(true)}
            onMouseLeave={() => setIsHovered(false)}
          >
            {isHovered ? <IoImage /> : <IoImageOutline />}
          </label>
          <input
            id="image-upload"
            type="file"
            accept="image/*"
            onChange={handleImageChange}
            style={{ display: "none" }}
          />
          <button className="publish-button" type="submit">
            Publish
          </button>
        </div>
      </form>
    </div>
  );
}

CreatePublication.propTypes = {
  onCloseClick: PropTypes.func.isRequired,
};

export default CreatePublication;
