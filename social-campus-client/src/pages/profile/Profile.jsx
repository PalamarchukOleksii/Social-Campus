import React, { useState, useEffect, useRef, useCallback } from "react";
import { useParams, Link, useNavigate } from "react-router-dom";
import "./Profile.css";
import PublicationsList from "../../components/publicationsList/PublicationsList";
import ROUTES from "../../utils/consts/Routes";
import Loading from "../../components/loading/Loading";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import useAuth from "../../hooks/useAuth";

const GET_USER_URL = "/api/users/by-login/";
const GET_USER_PUBLICATIONS_URL = "/api/users";

function Profile() {
  const { login } = useParams();
  const [user, setUser] = useState(null);
  const [publications, setPublications] = useState([]);
  const [loadingUser, setLoadingUser] = useState(true);
  const [loadingPublications, setLoadingPublications] = useState(false);
  const [hasMore, setHasMore] = useState(true);

  const navigate = useNavigate();
  const { auth } = useAuth();
  const axios = useAxiosPrivate();
  const observer = useRef();

  useEffect(() => {
    const fetchUserData = async () => {
      try {
        const { data } = await axios.get(`${GET_USER_URL}${login}`);
        setUser(data);
      } catch (error) {
        console.error("Error fetching user data:", error);
      } finally {
        setLoadingUser(false);
      }
    };

    fetchUserData();
  }, [login]);

  const fetchPublications = async () => {
    if (!user) return;
    setLoadingPublications(true);

    try {
      const limit = 2;
      const last = publications[publications.length - 1];
      const lastId = last?.id.value;

      const url = lastId
        ? `${GET_USER_PUBLICATIONS_URL}/${user.id.value}/publications/${limit}?lastPublicationId=${lastId}`
        : `${GET_USER_PUBLICATIONS_URL}/${user.id.value}/publications/${limit}`;

      const { data: newPublications } = await axios.get(url);

      setPublications((prev) => [...prev, ...newPublications]);
      setHasMore(newPublications.length === limit);
    } catch (err) {
      console.error("Failed to load publications", err);
    } finally {
      setLoadingPublications(false);
    }
  };

  const lastPublicationRef = useCallback(
    (node) => {
      if (loadingPublications) return;
      if (observer.current) observer.current.disconnect();

      observer.current = new IntersectionObserver((entries) => {
        if (entries[0].isIntersecting && hasMore) {
          fetchPublications();
        }
      });

      if (node) observer.current.observe(node);
    },
    [loadingPublications, hasMore, user]
  );

  useEffect(() => {
    if (user) {
      setPublications([]);
      fetchPublications();
    }
  }, [user]);

  if (loadingUser) {
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
      <div className="profile">
        <div className="profile-header">
          <div className="profile-image-container">
            <img
              src={user.profileImageUrl || "/default-profile.png"}
              alt="Profile"
              className="profile-image"
            />
          </div>
        </div>
        <div className="profile-info">
          <div className="short-info-cont">
            <div>
              <h2 className="profile-name general-text">
                {user.firstName} {user.lastName}
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
            <span>{user.followersCount} Followers</span>
          </Link>
          <Link to={ROUTES.FOLLOWING.replace(":login", user.login)}>
            <span>{user.followingCount} Following</span>
          </Link>
        </div>
      </div>

      <div className="publications">
        <PublicationsList
          publications={publications}
          lastPublicationRef={lastPublicationRef}
        />
        {loadingPublications && <p className="general-text">Loading...</p>}
      </div>
    </div>
  );
}

export default Profile;
