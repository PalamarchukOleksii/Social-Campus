import React, { useState, useEffect } from "react";
import userData from "../../data/userData.json";
import PublicationsList from "../../components/publicationsList/PublicationsList";
import login from "../../utils/consts/AuthUserLogin";
import Loading from "../../components/loading/Loading";

function Home() {
  const [publications, setPublications] = useState([]);
  const [loading, setLoading] = useState(true);
  const [currentUser, setCurrentUser] = useState(null);

  useEffect(() => {
    const fetchData = () => {
      const user = userData.find((user) => user.login === login);

      setCurrentUser(user);

      const followingIds = user.following.map(
        (followingUser) => followingUser.id
      );

      const followedPublications = userData
        .filter((user) => followingIds.includes(user.id))
        .flatMap((user) =>
          user.publications.map((publication) => ({
            ...publication,
            username: user.username,
            login: user.login,
            profileImage: user.profileImage,
          }))
        );

      const currentUserPublications = user.publications.map((publication) => ({
        ...publication,
        username: user.username,
        login: user.login,
        profileImage: user.profileImage,
      }));

      const allPublications = [
        ...currentUserPublications,
        ...followedPublications,
      ].sort((a, b) => new Date(b.creationTime) - new Date(a.creationTime));

      setPublications(allPublications);
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
