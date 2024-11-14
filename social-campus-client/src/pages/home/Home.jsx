import React, { useState, useEffect } from "react";
import userData from "../../data/userData.json";
import PublicationsList from "../../components/publicationsList/PublicationsList";
import login from "../../utils/consts/AuthUserLogin";
import Loading from "../../components/loading/Loading";
import "./Home.css";

function Home() {
  const [publications, setPublications] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = () => {
      const currentUser = userData.find((user) => user.login === login);

      const followingIds = currentUser.following.map(
        (followingUser) => followingUser.id
      );

      const followedPublications = userData
        .filter((user) => followingIds.includes(user.id))
        .flatMap((user) =>
          user.publications.map((publication) => ({
            ...publication,
            username: user.username,
            login: user.login,
            profileImage: user.profileImage || "/default-profile.png",
          }))
        )
        .sort((a, b) => new Date(b.creationTime) - new Date(a.creationTime));

      setPublications(followedPublications);
      setLoading(false);
    };

    fetchData();
  }, []);

  if (loading) {
    return (
      <div className="loading-container">
        <Loading />
      </div>
    );
  }

  if (publications.length === 0) {
    return (
      <h1 className="no-publications-text general-text">
        No followed publications found
      </h1>
    );
  }

  return (
    <div className="home">
      <PublicationsList publications={publications} />
    </div>
  );
}

export default Home;
