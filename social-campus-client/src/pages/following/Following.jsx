import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import FollowItem from "../../components/followItem/FollowItem";
import FollowTabs from "../../components/followTads/FollowTabs";
import "./Following.css";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";

const FOLLOWS_BASE_URL = "/api/follows";

function Following() {
  const axios = useAxiosPrivate();
  const { login } = useParams();
  const [following, setFollowing] = useState([]);

  useEffect(() => {
    const fetchFollowing = async () => {
      try {
        const response = await axios.get(
          `${FOLLOWS_BASE_URL}/${login}/following`
        );
        setFollowing(response.data);
      } catch (error) {
        console.error("Failed to fetch following:", error);
      }
    };

    fetchFollowing();
  }, [login]);

  const handleFollowClick = async (followUserLogin) => {
    try {
      await axios.post(`${FOLLOWS_BASE_URL}/follow`, {
        userLogin: login,
        followUserLogin,
      });
      setFollowing((prev) =>
        prev.map((user) =>
          user.username === followUserLogin
            ? { ...user, isFollowing: true }
            : user
        )
      );
    } catch (error) {
      console.error(`Failed to follow ${followUserLogin}:`, error);
    }
  };

  const handleUnfollowClick = async (followUserLogin) => {
    try {
      await axios.delete(
        `${FOLLOWS_BASE_URL}/unfollow/${login}/${followUserLogin}`
      );
      setFollowing((prev) =>
        prev.map((user) =>
          user.username === followUserLogin
            ? { ...user, isFollowing: false }
            : user
        )
      );
    } catch (error) {
      console.error(`Failed to unfollow ${followUserLogin}:`, error);
    }
  };

  return (
    <div className="container">
      <FollowTabs />
      {following.length > 0 ? (
        following.map((user) =>
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
        <h2 className="no-follow-text general-text">
          Not following anyone yet
        </h2>
      )}
    </div>
  );
}

export default Following;
