import React, { useState, useEffect, useCallback } from "react";
import { useParams } from "react-router-dom";
import FollowItem from "../../components/followItem/FollowItem";
import FollowTabs from "../../components/followTads/FollowTabs";
import "./Followers.css";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import useAuth from "../../hooks/useAuth";

const FOLLOWS_BASE_URL = "/api/follows";

function Followers() {
  const axios = useAxiosPrivate();
  const { auth } = useAuth();
  const { login } = useParams();
  const [followers, setFollowers] = useState([]);

  const fetchFollowers = useCallback(async () => {
    try {
      const response = await axios.get(
        `${FOLLOWS_BASE_URL}/${login}/followers`
      );
      setFollowers(response.data);
    } catch (error) {
      console.error("Failed to fetch followers:", error);
    }
  }, [axios, login]);

  useEffect(() => {
    fetchFollowers();
  }, [fetchFollowers]);

  const handleFollowClick = async (followUserLogin) => {
    try {
      await axios.post(`${FOLLOWS_BASE_URL}/follow`, {
        userLogin: auth.shortUser.login,
        followUserLogin,
      });
      fetchFollowers();
    } catch (error) {
      console.error(`Failed to follow ${followUserLogin}:`, error);
    }
  };

  const handleUnfollowClick = async (followUserLogin) => {
    try {
      await axios.delete(
        `${FOLLOWS_BASE_URL}/unfollow/${auth.shortUser.login}/${followUserLogin}`
      );
      fetchFollowers();
    } catch (error) {
      console.error(`Failed to unfollow ${followUserLogin}:`, error);
    }
  };

  return (
    <div className="container">
      <FollowTabs />
      {followers.length > 0 ? (
        followers.map((user) =>
          user.followersIds.some(
            (userId) => userId.value === auth.shortUser.id.value
          ) ? (
            <FollowItem
              key={user.id.value}
              userId={user.id.value}
              bio={user.bio}
              buttonText="Following"
              onClick={() => handleUnfollowClick(user.login)}
              hoveredText="Unfollow"
            />
          ) : (
            <FollowItem
              key={user.id.value}
              userId={user.id.value}
              bio={user.bio}
              buttonText="Follow"
              onClick={() => handleFollowClick(user.login)}
              hoveredText="Follow"
            />
          )
        )
      ) : (
        <h2 className="no-follow-text general-text">No followers yet</h2>
      )}
    </div>
  );
}

export default Followers;
