import React, { useState, useEffect } from "react";
import ShortProfile from "../shortProfile/ShortProfile";
import "./HorizontalRecommendedProfiles.css";

function HorizontalRecommendedProfiles() {
  const [recommendationList, setRecommendationList] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {}, []);

  if (loading) {
    return <></>;
  }

  if (!recommendationList || recommendationList.length === 0) {
    return (
      <h3 className="horizontal-not-found-text general-text">
        Recommendations not found
      </h3>
    );
  }

  return (
    <div className="horizontal-recommendation">
      <h2 className="horizontal-text general-text">Recommendations</h2>
      <div className="horizontal-list-container">
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

export default HorizontalRecommendedProfiles;
