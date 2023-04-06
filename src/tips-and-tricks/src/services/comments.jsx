import axios from 'axios';

import { API_URL } from '../utils/constants';

export async function sendComment(comment) {
	const { data } = await axios.post(
		`${API_URL}/comments?id=${comment.id}&name=${encodeURIComponent(
			comment.name,
		)}&description=${encodeURIComponent(comment.description)}`,
	);

	if (data.isSuccess) return data.result;
	else return null;
}
