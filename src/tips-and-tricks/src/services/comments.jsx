import axios from 'axios';

export async function sendComment(comment) {
	const { data } = await axios.post(
		`${process.env.REACT_APP_API_ENDPOINT}/comments?id=${
			comment.id
		}&name=${encodeURIComponent(
			comment.name,
		)}&description=${encodeURIComponent(comment.description)}`,
	);

	if (data.isSuccess) return data.result;
	else return null;
}
