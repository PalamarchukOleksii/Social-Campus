import React, { useState, useEffect, useRef, useCallback } from "react";
import PublicationsList from "../../components/publicationsList/PublicationsList";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import useAuth from "../../hooks/useAuth";
import Loading from "../../components/loading/Loading";

const GET_HOME_PAGE_PUBLICATIONS_BASE_URL = "/api/publications/home/user";

function Home() {
  const axios = useAxiosPrivate();
  const { auth } = useAuth();

  const [publications, setPublications] = useState([]);
  const [loading, setLoading] = useState(false);
  const [hasMore, setHasMore] = useState(true);

  const observer = useRef();

  const lastPublicationRef = useCallback(
    (node) => {
      if (loading) return;

      if (observer.current) observer.current.disconnect();

      observer.current = new IntersectionObserver((entries) => {
        if (entries[0].isIntersecting && hasMore) {
          fetchPublications();
        }
      });

      if (node) observer.current.observe(node);
    },
    [loading, hasMore]
  );

  const fetchPublications = async () => {
    setLoading(true);
    try {
      const userId = auth?.shortUser?.id.value;
      const limit = 2;
      const last = publications[publications.length - 1];
      const lastId = last?.id.value;

      const url = lastId
        ? `${GET_HOME_PAGE_PUBLICATIONS_BASE_URL}/${userId}/${limit}?lastPublicationId=${lastId}`
        : `${GET_HOME_PAGE_PUBLICATIONS_BASE_URL}/${userId}/${limit}`;

      const response = await axios.get(url);

      const newPublications = response.data;

      setPublications((prev) => [...prev, ...newPublications]);
      setHasMore(newPublications.length === limit);
    } catch (error) {
      console.error("Error fetching publications:", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchPublications();
  }, []);

  return (
    <div className="home">
      {publications.length === 0 && !loading ? (
        <h1 className="no-publications-text general-text">
          No followed publications found
        </h1>
      ) : (
        <PublicationsList
          publications={publications}
          lastPublicationRef={lastPublicationRef}
        />
      )}

      {loading && <Loading />}
    </div>
  );
}

export default Home;
