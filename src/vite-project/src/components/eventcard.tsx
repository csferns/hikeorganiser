import {AttendeeInformation, Event} from "../interfaces/GetAllEvents.ts"
import {Card, CardContent, Fade, Avatar, AvatarGroup, CardActions, Button, CardHeader, Badge, Modal, Box, Typography, Stack, Paper } from '@mui/material'

import { Done, QuestionMark, Clear } from '@mui/icons-material'
import {apiService} from "../defaults.ts";
import {useEffect, useState} from "react";
import dayjs from "dayjs";
import {
    APIProvider,
    AdvancedMarker,
    Map,
    useAdvancedMarkerRef,
} from '@vis.gl/react-google-maps';

export default function EventCard(props: { event: Event }) {
    
    const [userStatus, setUserStatus] = useState<number>();
    const [isMaximised, setIsMaximised] = useState<boolean>(false);
    const [markerRef, marker] = useAdvancedMarkerRef();
    
    const startDate : string = dayjs(props.event.startDate).format('MMMM D, YYYY [at] h:mm A');
    
    const rsvp = async (status : number) => {
        
        await apiService.rsvp(props.event.id, status);
        
        setUserStatus(status);
    }
    
    const getRSVPStatus = (user : AttendeeInformation)=> {
        switch (user.status) {
            case 1: return <Done color={"success"} />;
            case 2: return <QuestionMark color={"warning"} />;
            case 3: return <Clear color={"error"} />;
        } 
    }
    
    const getAvatar = (user : AttendeeInformation, showName: boolean) => {
        return <span style={{ display: "flex", gap: "20px", alignItems: "center"}}>
            <Badge badgeContent={getRSVPStatus(user)} anchorOrigin={{
                vertical: 'bottom',
                horizontal: 'right',
            }}>
                <Avatar alt={user.name}>
                    {user.name.slice(0, 1).toUpperCase()}
                </Avatar>
            </Badge>

            {showName && user.name}
        </span>
    }

    useEffect(() => {
        setUserStatus(props.event.currentUser.status);
    }, []);
    
    return (
        <>
            <Fade in>
                <Card onClick={() => setIsMaximised(true)}>
                    <CardHeader title={props.event.title}
                                subheader={startDate}/>
                    <CardContent>
                        <AvatarGroup max={4} spacing="small">
                            {props.event.attendees.map(a => getAvatar(a, false))}
                        </AvatarGroup>
                    </CardContent>
                    <CardActions>
                        <Button color={"success"} variant={userStatus === 1 ? "contained" : "text"} onClick={() => rsvp(1)}>
                            <Done />
                        </Button>
                        <Button color={"warning"} variant={userStatus === 2 ? "contained" : "text"} onClick={() => rsvp(2)}>
                            <QuestionMark />
                        </Button>
                        <Button color={"error"} variant={userStatus === 3 ? "contained" : "text"} onClick={() => rsvp(3)}>
                            <Clear />
                        </Button>
                    </CardActions>
                </Card>
            </Fade>
            
            <Modal open={isMaximised} 
                   onClose={() => setIsMaximised(false)}
                   aria-labelledby="modal-modal-title"
                   aria-describedby="modal-modal-description">
                <Box sx={{
                        position: 'absolute',
                        top: '50%',
                        left: '50%',
                        transform: 'translate(-50%, -50%)',
                        width: 400,
                        bgcolor: 'background.paper',
                        border: '2px solid #000',
                        boxShadow: 24,
                        p: 4,
                    }}>
                    <Typography id="modal-modal-title" component="h2">
                        { props.event.title }
                    </Typography>
                    <Typography id="modal-modal-description" sx={{ mt: 2 }}>
                        { startDate }
                    </Typography>

                    <Stack spacing={2}>
                        {getAvatar(props.event.currentUser, true)}
                        {props.event.attendees.sort(x => x.status).map(a => getAvatar(a, true))}
                    </Stack>

                    {props.event.location && props.event.location !== '[object Object]' && (
                        <>
                            <Paper variant="outlined" className="map-card" sx={{ borderRadius: 2, overflow: 'hidden', width: '100%' }}>
                                <APIProvider apiKey={import.meta.env.VITE_GOOGLE_MAPS_API_KEY}>
                                    <Map mapId={'bf51a910020fa25a'}
                                         defaultZoom={3}
                                         defaultCenter={{ lat: 22.54992, lng: 0 }}
                                         colorScheme={'DARK'}
                                         gestureHandling={'greedy'}
                                         disableDefaultUI={true}
                                         style={{ width: '100%', height: '300px' }}>

                                        <AdvancedMarker ref={markerRef} position={JSON.parse(props.event.location)} />
                                    </Map>
                                </APIProvider>
                            </Paper>
                        </>
                    )}
                    
                </Box>
            </Modal>
        </>
    )
}