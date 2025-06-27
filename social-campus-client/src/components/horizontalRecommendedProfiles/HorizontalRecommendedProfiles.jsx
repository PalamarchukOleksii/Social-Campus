import React, { useState, useEffect } from "react";
import ShortProfile from "../shortProfile/ShortProfile";
import "./HorizontalRecommendedProfiles.css";
import useAuth from "../../hooks/useAuth";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";

const BASE_USER_URL = "/api/users";

function HorizontalRecommendedProfiles() {
  const { auth } = useAuth();
  const axios = useAxiosPrivate();
  const [recommendationList, setRecommendationList] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    async function fetchRecommendations() {
      if (!auth?.shortUser?.id?.value) return;

      try {
        const response = await axios.get(
          `${BASE_USER_URL}/${auth.shortUser.id.value}/recommended-users`
        );
        setRecommendationList(response.data);
      } catch (error) {
        console.error("Failed to fetch recommended-users:", error);
      } finally {
        setLoading(false);
      }
    }

    fetchRecommendations();
  }, [auth]);

  if (loading) {
    return <></>;
  }

  return (
    <div className="horizontal-recommendation">
      <h2 className="horizontal-text general-text">Recommendations</h2>
      <div className="horizontal-list-container">
        {!recommendationList || recommendationList.length === 0 ? (
          <h3 className="not-found-text general-text">
            No recommendations found
          </h3>
        ) : (
          recommendationList.map((profile) => (
            <ShortProfile key={profile.id.value} userId={profile.id.value} />
          ))
        )}
      </div>
    </div>
  );
}

export default HorizontalRecommendedProfiles;
