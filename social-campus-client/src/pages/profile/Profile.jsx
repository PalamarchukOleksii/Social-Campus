import React, { useState, useEffect } from "react";
import { useParams, Link, useNavigate } from "react-router-dom";
import "./Profile.css";
import PublicationsList from "../../components/publicationsList/PublicationsList";
import ROUTES from "../../utils/consts/Routes";
import Loading from "../../components/loading/Loading";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import useAuth from "../../hooks/useAuth";

const GET_USER_URL = "/api/users/by-login/";
const GET_USER_PUBLICATIONS_URL = "/api/users";
const PAGE_SIZE = 10;

function Profile() {
  const { login } = useParams();
  const [user, setUser] = useState(null);
  const [publications, setPublications] = useState([]);
  const [loadingUser, setLoadingUser] = useState(true);
  const [loadingPublications, setLoadingPublications] = useState(false);
  const [allFetched, setAllFetched] = useState(false);

  const navigate = useNavigate();
  const { auth } = useAuth();
  const axios = useAxiosPrivate();

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
    if (loadingPublications || allFetched) return;

    setLoadingPublications(true);
    try {
      const last = publications[publications.length - 1];
      const lastId = last?.id.value;

      const url = lastId
        ? `${GET_USER_PUBLICATIONS_URL}/${user.id.value}/publications/count/${PAGE_SIZE}?lastPublicationId=${lastId}`
        : `${GET_USER_PUBLICATIONS_URL}/${user.id.value}/publications/count/${PAGE_SIZE}`;

      const { data: newPublications } = await axios.get(url);

      setPublications((prev) => [...prev, ...newPublications]);

      if (newPublications.length < PAGE_SIZE) {
        setAllFetched(true);
      }
    } catch (err) {
      console.error("Failed to load publications", err);
    } finally {
      setLoadingPublications(false);
    }
  };

  useEffect(() => {
    if (user) {
      setPublications([]);
      setAllFetched(false);
      fetchPublications();
    }
  }, [user, login]);

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
        <PublicationsList publications={publications} />
        {!allFetched && (
          <div className="load-more-container">
            <button onClick={fetchPublications} disabled={loadingPublications}>
              Load More
            </button>
          </div>
        )}
      </div>
    </div>
  );
}

export default Profile;
