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
import useAuth from "../../hooks/useAuth";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";

const CREATE_PUBLICATION_URL = "/api/publications/create";
const GET_PUBLICATION_URL = "/api/publications/";
const UPDATE_PUBLICATION_URL = "/api/publications/update";

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

  const axios = useAxiosPrivate();

  useEffect(() => {
    const currentUser = auth?.shortUser || {};
    setUser(currentUser);
    setLoading(false);
  }, [auth]);

  useEffect(() => {
    if (props.isForEdit && props.editPublicationId) {
      const fetchPublication = async () => {
        try {
          const response = await axios.get(
            `${GET_PUBLICATION_URL}${props.editPublicationId}`
          );
          const publication = response.data;
          setPublicationText(publication.description);

          if (publication.imageData) {
            setImagePreview(publication.imageData);
          }
        } catch (error) {
          console.error("Fetching publication data error:", error);
          toast.error("Failed to fetch the publication data.");
        }
      };

      fetchPublication();
    }
  }, [props.isForEdit, props.editPublicationId, axios]);

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

    try {
      const formData = new FormData();

      formData.append("description", publicationText);

      if (image) {
        formData.append("imageFile", image);
      } else {
        formData.append("imageFile", "");
      }

      if (props.isForEdit && props.editPublicationId) {
        formData.append("callerId.value", user.id.value);
        formData.append("publicationId.value", props.editPublicationId);

        await axios.patch(UPDATE_PUBLICATION_URL, formData, {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        });

        toast.success("Publication updated successfully.");
      } else {
        formData.append("creatorId.value", user.id.value);

        await axios.post(CREATE_PUBLICATION_URL, formData, {
          headers: {
            "Content-Type": "multipart/form-data",
          },
        });

        toast.success("Publication created successfully.");
      }
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
        userId={user.id.value}
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
            {props.isForEdit ? "Save Changes" : "Publish"}
          </button>
        </div>
      </form>
    </div>
  );
}

CreatePublication.propTypes = {
  onCloseClick: PropTypes.func.isRequired,
  isForEdit: PropTypes.bool,
  editPublicationId: PropTypes.string,
};

export default CreatePublication;
