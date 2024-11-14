import React, { useState, useEffect } from "react";
import { useParams, Link } from "react-router-dom";
import "./Profile.css";
import userData from "../../data/userData.json";
import PublicationsList from "../../components/publicationsList/PublicationsList";
import ROUTES from "../../utils/consts/Routes";
import Loading from "../../components/loading/Loading";

function Profile() {
  const { login } = useParams();
  const [user, setUser] = useState(null);
  const [publications, setPublications] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchUserData = () => {
      const foundUser = userData.find((user) => user.login === login);

      if (foundUser) {
        setUser(foundUser);
        setPublications(
          foundUser.publications
            .map((publication) => ({
              ...publication,
              username: foundUser.username,
              login: foundUser.login,
              profileImage: foundUser.profileImage || "/default-profile.png",
            }))
            .sort((a, b) => new Date(b.creationTime) - new Date(a.creationTime))
        );
      } else {
        setUser(null);
      }

      setLoading(false);
    };

    fetchUserData();
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
      <div className="profile">
        <div className="profile-header">
          <img
            src={user.profileImage || "/default-profile.png"}
            alt="Profile"
            className="profile-image"
          />
        </div>
        <div className="profile-info">
          <h2 className="profile-name general-text">{user.username}</h2>
          <p className="profile-login not-general-text">@{user.login}</p>
          <p className="profile-bio general-text">{user.bio}</p>
        </div>
        <div className="profile-stats">
          <Link to={ROUTES.FOLLOWERS.replace(":login", user.login)}>
            <span>{user.followers.length} Followers</span>
          </Link>
          <Link to={ROUTES.FOLLOWING.replace(":login", user.login)}>
            <span>{user.following.length} Following</span>
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
