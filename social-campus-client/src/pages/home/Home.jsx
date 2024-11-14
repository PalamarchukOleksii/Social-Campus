import React, { useState, useEffect } from "react";
import "./Home.css"; // Подключение CSS для страницы Home

import userData from "../../data/userData.json";
import Publication from "../../components/publication/Publication";


function Home() {
  const [publications, setPublications] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = () => {
      // Собираем все публикации всех пользователей
      const allPublications = userData.flatMap(user =>
        user.publications.map(publication => ({
          ...publication,
          username: user.username,
          login: user.login,
          profileImage: user.profileImage || "/default-profile.png" // Установка изображения профиля по умолчанию
        }))
      );
      setPublications(allPublications);
      setLoading(false);
    };

    fetchData();
  }, []);

  if (loading) {
    return <div>Loading...</div>; // Простое состояние загрузки
  }

  return (
    <div className="home">
      <h1>Home</h1>
      <div className="publications">
        {publications.map(publication => (
          <Publication
            key={publication.id}
            publicationId={publication.id}
            description={publication.description}
            imageUrl={publication.imageUrl}
            creationTime={publication.creationTime}
            likesCount={publication.likesCount}
            commentsCount={publication.comments.length}
            username={publication.username}
            login={publication.login}
            profileImage={publication.profileImage}
          />
        ))}
      </div>
    </div>
  );
}
export default Home;
