import http from 'k6/http';
import { check, sleep} from 'k6';

export const options = {
    stages: [
        { duration: '1m', target: 400 },
        { duration: '2m', target: 400 },
        { duration: '1m', target: 0 },
    ],

    thresholds: {
        http_req_duration: ['p(99)<15'], // At 400 VUs the threshold is exceeded but still no errors
    },
};

const BASE_URL = 'http://localhost:5205/api/v1';

export default () => {
    const guest = http.get(`${BASE_URL}/guest/9fc943e6-4fb2-4d35-a04c-3e46e81c5ac5`).json();
    check(guest, { 'id is not null': (obj) => obj.id !== null });
    sleep(1);
};