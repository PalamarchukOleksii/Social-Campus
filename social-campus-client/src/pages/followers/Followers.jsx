import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import FollowItem from "../../components/followItem/FollowItem";
import FollowTabs from "../../components/followTads/FollowTabs";
import userData from "../../data/userData.json";

function Followers() {
  const { login } = useParams();
  const [followers, setFollowers] = useState([]);

  useEffect(() => {
    const fetchUserData = () => {
      const user = userData.find((user) => user.login === login);
      if (user) {
        setFollowers(user.followers);
      }
    };

    fetchUserData();
  }, [login]);

  const handleFollowClick = (username) => {
    console.log(`Followed ${username}`);
    setFollowers((prev) =>
      prev.map((user) =>
        user.username === username ? { ...user, isFollowing: true } : user
      )
    );
  };

  const handleUnfollowClick = (username) => {
    console.log(`Unfollowed ${username}`);
    setFollowers((prev) =>
      prev.map((user) =>
        user.username === username ? { ...user, isFollowing: false } : user
      )
    );
  };

  return (
    <div className="page-container">
      <FollowTabs />
      {followers.length > 0 ? (
        followers.map((user) =>
          user.isFollowing ? (
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
          ) : (
            <FollowItem
              key={user.id}
              username={user.username}
              login={user.login}
              profileImage={user.profileImage}
              bio={user.bio}
              buttonText="Follow"
              onClick={() => handleFollowClick(user.username)}
              hoveredText="Follow"
            />
          )
        )
      ) : (
        <h2>No followers yet.</h2>
      )}
    </div>
  );
}

export default Followers;
