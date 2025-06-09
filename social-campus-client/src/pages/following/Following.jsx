import React, { useState, useEffect, useCallback } from "react";
import { useParams } from "react-router-dom";
import FollowItem from "../../components/followItem/FollowItem";
import FollowTabs from "../../components/followTads/FollowTabs";
import "./Following.css";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import useAuth from "../../hooks/useAuth";

const FOLLOWS_BASE_URL = "/api/follows";

function Following() {
  const axios = useAxiosPrivate();
  const { login } = useParams();
  const { auth } = useAuth();
  const [following, setFollowing] = useState([]);

  const fetchFollowing = useCallback(async () => {
    try {
      const response = await axios.get(
        `${FOLLOWS_BASE_URL}/${login}/following`
      );
      setFollowing(response.data);
    } catch (error) {
      console.error("Failed to fetch following:", error);
    }
  }, [axios, login]);

  useEffect(() => {
    fetchFollowing();
  }, [fetchFollowing]);

  const handleFollowClick = async (followUserLogin) => {
    try {
      await axios.post(`${FOLLOWS_BASE_URL}/follow`, {
        userLogin: auth.shortUser.login,
        followUserLogin,
      });
      fetchFollowing();
    } catch (error) {
      console.error(`Failed to follow ${followUserLogin}:`, error);
    }
  };

  const handleUnfollowClick = async (followUserLogin) => {
    try {
      await axios.delete(
        `${FOLLOWS_BASE_URL}/unfollow/${auth.shortUser.login}/${followUserLogin}`
      );
      fetchFollowing();
    } catch (error) {
      console.error(`Failed to unfollow ${followUserLogin}:`, error);
    }
  };

  return (
    <div className="container">
      <FollowTabs />
      {following.length > 0 ? (
        following.map((user) =>
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
        <h2 className="no-follow-text general-text">
          Not following anyone yet
        </h2>
      )}
    </div>
  );
}

export default Following;
