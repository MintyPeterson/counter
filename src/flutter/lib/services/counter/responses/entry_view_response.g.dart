// GENERATED CODE - DO NOT MODIFY BY HAND

part of 'entry_view_response.dart';

// **************************************************************************
// JsonSerializableGenerator
// **************************************************************************

EntryViewResponse _$EntryViewResponseFromJson(Map<String, dynamic> json) =>
    EntryViewResponse(
      entryId: json['entryId'] as String,
      entryDate: DateTime.parse(json['entryDate'] as String),
      entry: json['entry'] as int,
      notes: json['notes'] as String?,
    );
