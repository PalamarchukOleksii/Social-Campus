import React, { useState } from "react";
import SearchTabs from "../../components/searchTabs/SearchTabs";
import userData from "../../data/userData.json";
import "./UsersSearch.css";
import FollowItem from "../../components/followItem/FollowItem";
import AuthUserLogin from "../../utils/consts/AuthUserLogin";

function UsersSearch() {
  const [searchQuery, setSearchQuery] = useState("");
  const authUser = userData.find((user) => user.login === AuthUserLogin);

  const [followStatus, setFollowStatus] = useState(() => {
    const initialStatus = {};
    if (authUser && authUser.following) {
      authUser.following.forEach((followedUser) => {
        initialStatus[followedUser.id] = true;
      });
    }
    return initialStatus;
  });

  const filteredUsers = userData.filter(
    (user) =>
      user.login !== AuthUserLogin &&
      (user.username.toLowerCase().includes(searchQuery.toLowerCase()) ||
        user.login.toLowerCase().includes(searchQuery.toLowerCase()))
  );

  const handleFollowClick = (userId) => {
    setFollowStatus((prevStatus) => ({
      ...prevStatus,
      [userId]: !prevStatus[userId],
    }));
  };

  return (
    <div className="search-page-container">
      <input
        type="text"
        placeholder="Type to search for users..."
        className="search-input"
        value={searchQuery}
        onChange={(e) => setSearchQuery(e.target.value)}
      />
      <SearchTabs />
      <div className="users-list">
        {filteredUsers.length > 0 ? (
          filteredUsers.map((user) => (
            <FollowItem
              key={user.id}
              username={user.username}
              login={user.login}
              profileImage={user.profileImage}
              bio={user.bio}
              buttonText={followStatus[user.id] ? "Following" : "Follow"}
              hoveredText={followStatus[user.id] ? "Unfollow" : "Follow"}
              onClick={() => handleFollowClick(user.id)}
            />
          ))
        ) : (
          <h2 className="not-found-users general-text">No users found.</h2>
        )}
      </div>
    </div>
  );
}

export default UsersSearch;
