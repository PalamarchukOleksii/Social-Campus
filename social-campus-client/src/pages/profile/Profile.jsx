import React, { useState, useEffect } from "react";
import { useParams, Link, useNavigate } from "react-router-dom";
import "./Profile.css";
import PublicationsList from "../../components/publicationsList/PublicationsList";
import ROUTES from "../../utils/consts/Routes";
import Loading from "../../components/loading/Loading";
import { useCreateItem } from "../../context/CreateItemContext";
import CreatePublication from "../../components/createPublication/CreatePublication";
import getMaxPublicationId from "../../utils/helpers/GetMaxPublicationId";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import useAuth from "../../hooks/useAuth";

const GET_USER_URL = "/api/users";

function Profile() {
  const { login } = useParams();
  const [user, setUser] = useState(null);
  const [publications, setPublications] = useState([]);
  const [loading, setLoading] = useState(true);

  const navigate = useNavigate();
  const { auth } = useAuth();
  const { isCreatePublicationOpen, closeCreatePublication } = useCreateItem();
  const axios = useAxiosPrivate();

  useEffect(() => {
    let isMounted = true;
    const controller = new AbortController();

    const fetchUserData = async () => {
      try {
        const { data } = await axios.get(`${GET_USER_URL}/${login}`, {
          signal: controller.signal,
        });
        if (isMounted) {
          setUser(data);
          setPublications(data.publications || []);
        }
      } catch (error) {
        if (error.name !== "CanceledError") {
          console.error("Error fetching user data:", error);
        }
      } finally {
        if (isMounted) {
          setLoading(false);
        }
      }
    };

    fetchUserData();

    return () => {
      isMounted = false;
      controller.abort();
    };
  }, [login]);

  if (loading) {
    return (
      <div className="loading-container">
        <Loading />
      </div>
    );
  }

  if (!user) {
    return <h1 className="not-found-text general-text">User not found</h1>;
  }

  return (
    <div className="wrapper">
      {isCreatePublicationOpen && (
        <div className="create-publication-modal-overlay">
          <CreatePublication
            publications={auth.shortUser.login === login ? publications : []}
            setPublications={
              auth.shortUser.login === login ? setPublications : null
            }
            getMaxPublicationId={getMaxPublicationId}
            close={closeCreatePublication}
          />
        </div>
      )}
      <div className="profile">
        <div className="profile-header">
          <div className="profile-image-container">
            <img
              src={user.profileImage || "/default-profile.png"}
              alt="Profile"
              className="profile-image"
            />
          </div>
        </div>
        <div className="profile-info">
          <div className="short-info-cont">
            <div>
              <h2 className="profile-name general-text">
                {user.firstName + " " + user.lastName}
              </h2>
              <p className="profile-login not-general-text">@{user.login}</p>
            </div>
            {user.login === auth.shortUser.login ? (
              <button
                onClick={() => navigate(`/profile/${user.login}/edit`)}
                className="edit-profile-link"
              >
                Edit Profile
              </button>
            ) : (
              <button
                onClick={() => navigate(`/messages/${user.login}`)}
                className="edit-profile-link"
              >
                Send Message
              </button>
            )}
          </div>
          <p className="profile-bio general-text">{user.bio}</p>
        </div>
        <div className="profile-stats">
          <Link to={ROUTES.FOLLOWERS.replace(":login", user.login)}>
            <span>{user.followers?.length || 0} Followers</span>
          </Link>
          <Link to={ROUTES.FOLLOWING.replace(":login", user.login)}>
            <span>{user.following?.length || 0} Following</span>
          </Link>
        </div>
      </div>
      <div className="publications">
        {publications.length > 0 ? (
          <PublicationsList publications={publications} />
        ) : (
          <h2 className="no-publications-text general-text">
            No publications yet
          </h2>
        )}
      </div>
    </div>
  );
}

export default Profile;
