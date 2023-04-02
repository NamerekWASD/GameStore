import { useState } from "react";

const videos = [
    "https://localhost:7219/files/game%20videos/static/Doom.mp4",
    "https://localhost:7219/files/game%20videos/static/Horizen.mp4",
    "https://localhost:7219/files/game%20videos/static/EldenRing.mp4",
    "https://localhost:7219/files/game%20videos/static/TheWitcher3.mp4",
]

export const VideoContainer = () => {
    const [currentVideoIndex, setCurrentVideoIndex] = useState(() => Math.floor(Math.random() * videos.length));

    const handleEnded = () => setCurrentVideoIndex(index => {
        const randomIndex = Math.floor(Math.random() * videos.length);
        if(randomIndex === index){
            return index === videos.length - 1 ? 0 : index + 1
        }
        return randomIndex;
    });

    return (
        <>
            <video
                src={`${videos[currentVideoIndex]}`}
                type="video/mp4"
                autoPlay
                muted
                onEnded={handleEnded}
            />
        </>
    )
}