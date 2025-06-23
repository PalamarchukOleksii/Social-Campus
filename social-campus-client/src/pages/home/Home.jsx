import React, { useState, useEffect } from "react";
import PublicationsList from "../../components/publicationsList/PublicationsList";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import useAuth from "../../hooks/useAuth";
import Loading from "../../components/loading/Loading";
import "./Home.css";

const GET_HOME_PAGE_PUBLICATIONS_BASE_URL = "/api/publications/home/user";
const PAGE_SIZE = 10;

function Home() {
  const axios = useAxiosPrivate();
  const { auth } = useAuth();

  const [page, setPage] = useState(1);
  const [publications, setPublications] = useState([]);
  const [loading, setLoading] = useState(true);
  const [allFetched, setAllFetched] = useState(false);

  const fetchPublications = async () => {
    try {
      const response = await axios.get(
        `${GET_HOME_PAGE_PUBLICATIONS_BASE_URL}/${auth.shortUser.id.value}/count/${PAGE_SIZE}/page/${page}`
      );
      const newPublications = response.data;

      if (newPublications.length === 0) {
        setAllFetched(true);
        return;
      }

      setPublications((prev) => [...prev, ...newPublications]);
      setPage((prev) => prev + 1);
    } catch (error) {
      console.error("Error fetching publications:", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchPublications();
  }, []);

  if (loading && publications.length === 0) {
    return <Loading />;
  }

  return (
    <div className="home">
      <PublicationsList publications={publications} />
      {!allFetched && (
        <div className="load-more-container">
          <button onClick={fetchPublications}>Load More</button>
        </div>
      )}
    </div>
  );
}

export default Home;
