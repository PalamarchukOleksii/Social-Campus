import React, { useState } from "react";
import FollowItem from "../../components/followItem/FollowItem";
import followersData from "../../data/followersData.json";
import FollowTabs from "../../components/followTads/FollowTabs";

function Followers() {
  const [followers, setFollowers] = useState(followersData);

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
