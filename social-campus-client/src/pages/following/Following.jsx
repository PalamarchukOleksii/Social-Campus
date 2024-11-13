import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import FollowItem from "../../components/followItem/FollowItem";
import FollowTabs from "../../components/followTads/FollowTabs";
import userData from "../../data/userData.json";
import "./Following.css";

function Following() {
  const { login } = useParams();
  const [following, setFollowing] = useState([]);

  useEffect(() => {
    const fetchUserData = () => {
      const user = userData.find((user) => user.login === login);
      if (user) {
        setFollowing(user.following);
      }
    };

    fetchUserData();
  }, [login]);

  const handleUnfollowClick = (username) => {
    console.log(`Unfollowed ${username}`);
    setFollowing((prev) =>
      prev.map((user) =>
        user.username === username ? { ...user, isFollowing: false } : user
      )
    );
  };

  return (
    <div className="container">
      <FollowTabs />
      {following.some((user) => user.isFollowing) ? (
        following.map(
          (user) =>
            user.isFollowing && (
              <FollowItem
                key={user.id}
                username={user.username}
                login={user.login}
                profileImage={user.profileImage}
                bio={user.bio}
                buttonText="Following"
                onClick={() => handleUnfollowClick(user.username)}
                hoveredText="Unfollow"
              />
            )
        )
      ) : (
        <h2 className="no-follow-text general-text">
          Not following anyone yet
        </h2>
      )}
    </div>
  );
}

export default Following;
