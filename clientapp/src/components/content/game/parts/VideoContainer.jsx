import { useEffect, useState } from "react";

export const VideoContainer = () => {
    const [videos, setVideos] = useState([]);
    
    const [currentVideoIndex, setCurrentVideoIndex] = useState(() => Math.floor(Math.random() * videos.length));
    
    useEffect(() => {
        fetch('api/WWWroot').then(response => response.json())
        .then(result => {
            setVideos(result);
        });
    }, [])

    const handleEnded = () => setCurrentVideoIndex(index => {
        const randomIndex = Math.floor(Math.random() * videos.length);
        if(randomIndex === index){
            return index === videos.length - 1 ? 0 : index + 1
        }
        return randomIndex;
    });

    return (
        <>
        {
            videos ? 
            <video
                src={videos[currentVideoIndex]}
                type="video/mp4"
                autoPlay
                muted
                onEnded={handleEnded}
            />
            :""
        }
        </>
    )
}