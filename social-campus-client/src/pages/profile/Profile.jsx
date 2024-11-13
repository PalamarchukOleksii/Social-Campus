import React, { useState, useEffect } from "react";
import { useParams, Link } from "react-router-dom";
import "./Profile.css";
import userData from "../../data/userData.json";
import Publication from "../../components/publication/Publication";
import ROUTES from "../../utils/consts/Routes";

function Profile() {
  const { login } = useParams();
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchUserData = () => {
      const foundUser = userData.find((user) => user.login === login);
      setUser(foundUser || null);
      setLoading(false);
    };

    window.scrollTo(0, 0);

    fetchUserData();
  }, [login]);

  if (loading) {
    return <p>Loading user data...</p>;
  }

  if (!user) {
    return <p>User not found.</p>;
  }

  return (
    <div className="wraper">
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
        {user.publications.map((publication, index) => (
          <Publication
            key={index}
            publicationId={publication.id}
            description={publication.description}
            imageUrl={publication.imageUrl}
            creationTime={publication.creationTime}
            likesCount={publication.likesCount}
            commentsCount={publication.comments.length}
            username={user.username}
            login={user.login}
            profileImage={user.profileImage}
          />
        ))}
      </div>
    </div>
  );
}

export default Profile;
