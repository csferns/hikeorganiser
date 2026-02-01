import { useState, useEffect, useRef } from 'react'
import {
    TextField,
    FormControl,
    FormControlLabel,
    Button,
    Switch,
    Stack,
    Select,
    MenuItem,
    InputLabel,
    Container,
    Paper,
    Typography,
    Box,
    Alert,
    styled
} from '@mui/material'
import { DateTimePicker } from '@mui/x-date-pickers';
import { Save } from '@mui/icons-material'
import dayjs, { Dayjs } from 'dayjs';
import withReactContent from 'sweetalert2-react-content'
import ScheduleRequest from "../interfaces/ScheduleRequest.ts";
import '../toasts.css'
import { apiService, Toast } from '../defaults.ts';
import { 
    APIProvider,
    ControlPosition,
    MapControl,
    AdvancedMarker,
    Map,
    useMap,
    useMapsLibrary,
    useAdvancedMarkerRef,
} from '@vis.gl/react-google-maps';
import GetAllBucketLists from "../interfaces/GetAllBucketLists.ts";

export default function Schedule() {
    
    const [startDate, setStartDate] = useState<Dayjs>(dayjs(new Date()));
    const [endDate, setEndDate] = useState<Dayjs>();
    
    const [bucketList, setBucketList] = useState<GetAllBucketLists>();
    const [selectedBucketList, setSelectedBucketList] = useState<number | null>(null);
    
    const [title, setTitle] = useState<string>("");
    const [description, setDescription] = useState<string>();
    const [location, setLocation] = useState<google.maps.places.PlaceResult | null>(null);
    const [markerRef, marker] = useAdvancedMarkerRef();
    const [hasEndDate, setHasEndDate] = useState<boolean>(false);

    const yesterday = dayjs().add(-1, 'day');
    
    function ScheduleEvent() : void {

        const request : ScheduleRequest = {
            Title: title,
            Description: description,
            Location: JSON.stringify(location?.geometry?.location),
            StartDate: startDate,
            EndDate: hasEndDate ? endDate : undefined,
            BucketListId: selectedBucketList
        };

        apiService.scheduleEvent(request)
            .then((_) => {

                withReactContent(Toast).fire({
                    title: `Saved ${title}`,
                    icon: 'success',
                });
            })
            .catch((e) => {
                console.error(e);

                withReactContent(Toast).fire({
                    title: 'Error',
                    icon: 'error',
                    text: e
                });
            });
    }

    useEffect(() => {
        
        apiService.getBucketList()
            .then(setBucketList)
    }, []);

    return (
        <Box sx={{ py: 4 }}>
            <Container maxWidth="md">
                <Stack spacing={2}>
                    <Typography variant="h4" component="h1" fontWeight={600}>
                        Schedule an event
                    </Typography>

                    <Paper className="page-card" elevation={4} sx={{ p: 3, borderRadius: 2 }}>
                        <Stack spacing={{ xs: 1, sm: 2 }}
                               direction="row"
                               useFlexGap
                               sx={{ flexWrap: 'wrap' }}>
                            <TextField fullWidth label="Title" value={title} onChange={(e) => setTitle(e.target.value)} />
                            <TextField fullWidth multiline minRows={3} label="Description" value={description} onChange={(e) => setDescription(e.target.value)} />
                            <FormControl fullWidth>
                                <InputLabel id="bucket-list-label">From bucket list (optional)</InputLabel>
                                <Select labelId="bucket-list-label" label="From bucket list (optional)" onChange={(e) => setSelectedBucketList(e.target.value as number || 0)} value={selectedBucketList ?? ''}>
                                    <MenuItem value="">
                                        <em>None</em>
                                    </MenuItem>
                                    {bucketList?.items?.map(x => (
                                        <MenuItem value={x.id} key={x.id}>{x.friendlyName}</MenuItem>
                                    ))}
                                </Select>
                            </FormControl>

                            {(selectedBucketList === null || selectedBucketList == undefined || selectedBucketList === 0) && (
                                <>
                                    <Typography variant="subtitle1" sx={{ mb: 1 }}>Location</Typography>
                                    <Paper variant="outlined" className="map-card" sx={{ borderRadius: 2, overflow: 'hidden' }}>
                                        <APIProvider apiKey={import.meta.env.VITE_GOOGLE_MAPS_API_KEY}>
                                            <Map mapId={'bf51a910020fa25a'}
                                                 defaultZoom={3}
                                                 defaultCenter={{ lat: 22.54992, lng: 0 }}
                                                 colorScheme={'DARK'}
                                                 gestureHandling={'greedy'}
                                                 disableDefaultUI={true}
                                                 style={{ width: '100%', height: '300px' }}
                                                 onClick={(e) => {
                                                     const latLng = e.detail.latLng;
                                                     if (!latLng) return;
                                                     // Move marker to clicked position
                                                     if (marker) {
                                                         marker.position = latLng;
                                                     }
                                                     // Update location state with minimal PlaceResult containing geometry.location
                                                     setLocation({
                                                         geometry: { location: latLng }
                                                     });
                                                 }}>
                                                <MapControl position={ControlPosition.TOP}
                                                            // basic styling to make the input visible on the map
                                                >
                                                    <div style={{ padding: 8 }}>
                                                        <PlaceAutocomplete onPlaceSelect={setLocation} />
                                                    </div>
                                                </MapControl>
                                                <MapHandler place={location} marker={marker} />
                                                <AdvancedMarker ref={markerRef} position={null} />
                                            </Map>
                                        </APIProvider>
                                    </Paper>
                                </>
                            )}
                            <FormControlLabel control={<Switch checked={hasEndDate} onChange={e => setHasEndDate(e.target.checked)} />} label="Repeating" />
                            <DateTimePicker label={hasEndDate ? 'Start date' : 'Date'} value={startDate} onChange={(d) => d && setStartDate(d)} minDate={yesterday} sx={{ width: '100%' }} />
                            {hasEndDate && (
                                    <DateTimePicker label="End date" value={endDate} onChange={(d) => d && setEndDate(d)} sx={{ width: '100%' }} />
                            )}
                            <Stack direction="row" justifyContent="flex-end">
                                <Button variant="contained" startIcon={<Save />} color="success" onClick={ScheduleEvent}>Save</Button>
                            </Stack>
                        </Stack>
                    </Paper>
                </Stack>
            </Container>
        </Box>
    )
}

interface MapHandlerProps {
    place: google.maps.places.PlaceResult | null;
    marker: google.maps.marker.AdvancedMarkerElement | null;
}

const MapHandler = ({ place, marker }: MapHandlerProps) => {
    const map = useMap();

    useEffect(() => {
        if (!map || !place || !marker) return;

        if (place.geometry?.viewport) {
            map.fitBounds(place.geometry?.viewport);
        }
        marker.position = place.geometry?.location;
    }, [map, place, marker]);

    return null;
};

interface PlaceAutocompleteProps {
    onPlaceSelect: (place: google.maps.places.PlaceResult | null) => void;
}

const PlaceAutocomplete = ({ onPlaceSelect }: PlaceAutocompleteProps) => {
    const [placeAutocomplete, setPlaceAutocomplete] =
        useState<google.maps.places.Autocomplete | null>(null);
    const inputRef = useRef<HTMLInputElement>(null);
    const places = useMapsLibrary('places');

    useEffect(() => {
        if (!places || !inputRef.current) return;

        const options = {
            fields: ['geometry', 'name', 'formatted_address']
        };

        setPlaceAutocomplete(new places.Autocomplete(inputRef.current, options));
    }, [places]);

    useEffect(() => {
        if (!placeAutocomplete) return;

        placeAutocomplete.addListener('place_changed', () => {
            onPlaceSelect(placeAutocomplete.getPlace());
        });
    }, [onPlaceSelect, placeAutocomplete]);

    return (
        <div className="autocomplete-container">
            <input ref={inputRef} />
        </div>
    );
};