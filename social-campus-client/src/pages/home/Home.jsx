import React, { useState, useEffect } from "react";
import PublicationsList from "../../components/publicationsList/PublicationsList";

function Home() {
  const [publications, setPublications] = useState([]);

  useEffect(() => {}, []);

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
