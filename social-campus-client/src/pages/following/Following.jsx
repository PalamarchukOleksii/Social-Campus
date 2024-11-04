import React, { useState } from "react";
import FollowItem from "../../components/followItem/FollowItem";
import followingData from "../../data/followingData.json";
import FollowTabs from "../../components/followTads/FollowTabs";

function Following() {
  const [following, setFollowing] = useState(followingData);

  const handleUnfollowClick = (username) => {
    console.log(`Unfollowed ${username}`);
    setFollowing((prev) =>
      prev.map((user) =>
        user.username === username ? { ...user, isFollowing: false } : user
      )
    );
  };

  return (
    <div className="page-container">
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
        <h2>Not following anyone yet.</h2>
      )}
    </div>
  );
}

export default Following;
