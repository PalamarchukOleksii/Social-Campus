import React, { useState, useEffect } from "react";
import ShortProfile from "../shortProfile/ShortProfile";
import userData from "../../data/userData.json";
import login from "../../utils/consts/AuthUserLogin";
import "./RecommendedProfiles.css";

function RecommendedProfiles() {
  const [recommendationList, setRecommendationList] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchUserData = () => {
      const user = userData.find((user) => user.login === login);

      if (user) {
        const notFollowList = user.followers.filter(
          (follower) => !follower.isFollowing
        );
        setRecommendationList(notFollowList);
      } else {
        setRecommendationList(null);
      }
      setLoading(false);
    };

    fetchUserData();
  }, []);

  if (loading) {
    return <></>;
  }

  if (!recommendationList || recommendationList.length === 0) {
    return (
      <h3 className="not-found-text general-text">Recommendations not found</h3>
    );
  }

  return (
    <div className="recommendation">
      <h2 className="text general-text">Recommendations</h2>
      <div className="list-container">
        {recommendationList.map((profile) => (
          <ShortProfile
            key={profile.id}
            username={profile.username}
            login={profile.login}
            profileImage={profile.profileImage}
          />
        ))}
      </div>
    </div>
  );
}

export default RecommendedProfiles;
