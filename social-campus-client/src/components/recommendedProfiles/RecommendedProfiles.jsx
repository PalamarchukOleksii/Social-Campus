import React, { useState, useEffect } from "react";
import ShortProfile from "../shortProfile/ShortProfile";
import "./RecommendedProfiles.css";
import useAuth from "../../hooks/useAuth";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import Loading from "../loading/Loading";

const BASE_USER_URL = "/api/users";

function RecommendedProfiles() {
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
    return <Loading />;
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
          <ShortProfile key={profile.id.value} userId={profile.id.value} />
        ))}
      </div>
    </div>
  );
}

export default RecommendedProfiles;
