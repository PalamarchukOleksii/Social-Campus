import React, { useState, useEffect } from "react";
import ShortProfile from "../shortProfile/ShortProfile";
import "./RecommendedProfiles.css";

function RecommendedProfiles() {
  const [recommendationList, setRecommendationList] = useState([]);

  useEffect(() => {}, []);

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
