import axios from 'axios';

import { API_URL } from '../utils/constants';

export async function sendComment(comment) {
	const response = await axios.post(
		`${API_URL}/api/comments?id=${comment.id}&name=${encodeURIComponent(
			comment.name,
		)}&description=${encodeURIComponent(comment.description)}`,
	);
	const data = response.data;

	if (data.isSuccess) return data.result;
	else return null;
}
